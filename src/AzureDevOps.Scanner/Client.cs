using AzureDevOps.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureDevOps.Scanner
{
    public class Client
    {
        private static readonly HttpClient RestClient = new HttpClient();

        private static int ProjectsDoneCount = 0;

        public Client(string pat)
        {
            RestClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            RestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", pat))));
        }

        public async Task<AzureDevOpsInstance> ScanAsync(DataOptions dataOptions, IEnumerable<string> collections, string azureDevOpsUrl)
        {
            ProjectsDoneCount = 0;

            var azureDevOpsInstance = new AzureDevOpsInstance();

            if (azureDevOpsUrl.ToLower().StartsWith("https://dev.azure.com"))
            {
                // There should be only one Org, since a PAT is limited to Organisation
                var organisation = collections.Single();

                Console.WriteLine($"Scanning organisation: [{organisation}]");
                azureDevOpsInstance.Collections.Add(await ScanCollectionAsync(dataOptions, organisation, azureDevOpsUrl));
            }
            else
            {
                var collectionTasks = new HashSet<Task<AzureDevOpsCollection>>();

                foreach (var collection in collections)
                {
                    Console.WriteLine($"Scanning collection: [{collection}]");
                    collectionTasks.Add(ScanCollectionAsync(dataOptions, collection, azureDevOpsUrl));
                }
                azureDevOpsInstance.Collections.AddRange(await Task.WhenAll(collectionTasks));
            }

            return azureDevOpsInstance;
        }

        private async Task<AzureDevOpsCollection> ScanCollectionAsync(DataOptions dataOptions, string collection, string azureDevOpsUrl)
        {
            HttpResponseMessage httpProjectResponse = await RestClient.GetAsync($"{azureDevOpsUrl}/{collection}/_apis/projects").ConfigureAwait(false);
            var responseProjectContent = await httpProjectResponse.Content.ReadAsStringAsync();
            var projectResult = JsonConvert.DeserializeObject<AzureDevOpsProjects>(responseProjectContent);

            var azDOCollection = new AzureDevOpsCollection { Name = collection };
            var scanProjectTasks = new HashSet<Task<AzureDevOpsProject>>();

            Console.WriteLine($"Scanning [{projectResult.Count}] projects");

            foreach (var project in projectResult.Projects)
            {
                scanProjectTasks.Add(ScanProjectAsync(dataOptions, $"{azureDevOpsUrl}/{collection}/{project.Id}", project));
            }

            azDOCollection.Projects.AddRange(await Task.WhenAll(scanProjectTasks));

            Console.WriteLine();

            return azDOCollection;
        }

        private static async Task<AzureDevOpsProject> ScanProjectAsync(DataOptions dataOptions, string projectUrl, AzureDevOpsProject project)
        {
            var buildScanTasks = ScanBuildsAsync(dataOptions, projectUrl);
            var releaseScanTasks = ScanReleasesAsync(dataOptions, projectUrl);
            var repositoryScanTask = ScanRepositoriesAsync(dataOptions, projectUrl);

            project.Builds = await buildScanTasks;
            project.Releases = await releaseScanTasks;
            project.Repositories = await repositoryScanTask;

            ProjectsDoneCount++;
            Console.Write(ProjectsDoneCount % 10 == 0 ? "*" : ".");

            return project;
        }
        private static async Task<IEnumerable<AzureDevOpsBuild>> ScanBuildsAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Build) ||
                dataOptions.HasFlag(DataOptions.BuildArtifacts)))
            {
                return Array.Empty<AzureDevOpsBuild>();
            }

            HttpResponseMessage httpBuildResponse = await RestClient.GetAsync($"{projectUrl}/_apis/build/builds").ConfigureAwait(false);
            var responseBuildContent = await httpBuildResponse.Content.ReadAsStringAsync();
            var buildresult = JsonConvert.DeserializeObject<AzureDevOpsBuilds>(responseBuildContent);

            var builds = Array.Empty<AzureDevOpsBuild>();
            if (buildresult.Count > 0)
            {
                builds = buildresult.Builds.ToArray();
                if (dataOptions.HasFlag(DataOptions.BuildArtifacts))
                {
                    var buildScanTasks = new HashSet<Task<AzureDevOpsBuild>>();
                    foreach (var build in builds)
                    {
                        buildScanTasks.Add(ScanBuildAsync(build));
                    }
                    builds = await Task.WhenAll(buildScanTasks);
                }
            }
            return builds;
        }

        private static async Task<AzureDevOpsBuild> ScanBuildAsync(AzureDevOpsBuild build)
        {
            HttpResponseMessage httpArtifactResponse = await RestClient.GetAsync($"{build.Url}/artifacts?api-version=5.1").ConfigureAwait(false);
            var artifactResponseContent = await httpArtifactResponse.Content.ReadAsStringAsync();
            build.Artifacts = JsonConvert.DeserializeObject<AzureDevOpsBuildArtifacts>(artifactResponseContent).Artifacts;
            return build;
        }

        private static async Task<IEnumerable<AzureDevOpsRelease>> ScanReleasesAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Release) ||
                dataOptions.HasFlag(DataOptions.ReleaseDetails)))
            {
                return Array.Empty<AzureDevOpsRelease>();
            }

            // Azure DevOps services uses a different host for release management Rest calls
            projectUrl = projectUrl.ToLower().Replace("https://dev.azure.com", "https://vsrm.dev.azure.com");
            HttpResponseMessage httpReleaseResponse = await RestClient.GetAsync($"{projectUrl}/_apis/release/releases").ConfigureAwait(false);
            var responseReleaseContent = await httpReleaseResponse.Content.ReadAsStringAsync();
            var releaseresult = JsonConvert.DeserializeObject<AzureDevOpsReleases>(responseReleaseContent);

            var releases = Array.Empty<AzureDevOpsRelease>();
            if (releaseresult.Count > 0)
            {
                releases = releaseresult.Releases.ToArray();
                if (dataOptions.HasFlag(DataOptions.ReleaseDetails)) // DataOption detailscan
                {
                    var releaseScanTasks = new HashSet<Task<AzureDevOpsRelease>>();
                    foreach (var release in releases)
                    {
                        releaseScanTasks.Add(ScanReleaseDetailAsync(release));
                    }
                    releases = await Task.WhenAll(releaseScanTasks);
                }
            }
            return releases;
        }

        private static async Task<AzureDevOpsRelease> ScanReleaseDetailAsync(AzureDevOpsRelease release)
        {
            HttpResponseMessage httpReleaseDetailResponse = await RestClient.GetAsync($"{release.Url}").ConfigureAwait(false);
            var releaseDetailResponseContent = await httpReleaseDetailResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AzureDevOpsRelease>(releaseDetailResponseContent);
        }

        private static async Task<IEnumerable<AzureDevOpsRepository>> ScanRepositoriesAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Git) ||
                dataOptions.HasFlag(DataOptions.GitPolicies)))
            {
                return Array.Empty<AzureDevOpsRepository>();
            }

            HttpResponseMessage httpRepositoriesResponse = await RestClient.GetAsync($"{projectUrl}/_apis/git/repositories?api-version=5.0").ConfigureAwait(false);
            var policiesResponseContent = await httpRepositoriesResponse.Content.ReadAsStringAsync();
            var repositoryResult = JsonConvert.DeserializeObject<AzureDevOpsRepositories>(policiesResponseContent);

            var repositories = Array.Empty<AzureDevOpsRepository>();
            if (repositoryResult.Count > 0)
            {
                repositories = repositoryResult.Repositories.ToArray();
                if (dataOptions.HasFlag(DataOptions.GitPolicies)) // DataOption policies
                {
                    var policyscanTasks = new HashSet<Task<AzureDevOpsRepository>>();
                    foreach (var repository in repositories)
                    {
                        policyscanTasks.Add(ScanBranchPoliciesAsync(projectUrl, repository));
                    }
                    repositories = await Task.WhenAll(policyscanTasks);
                }
            }
            return repositories;
        }

        private static async Task<AzureDevOpsRepository> ScanBranchPoliciesAsync(string projectUrl, AzureDevOpsRepository repository, string branchName = "refs/heads/master")
        {
            HttpResponseMessage httpPoliciesResponse = await RestClient.GetAsync($"{projectUrl}/_apis/git/policy/configurations?repositoryId={repository.Id}&refName={branchName}").ConfigureAwait(false);
            var policiesResponseContent = await httpPoliciesResponse.Content.ReadAsStringAsync();
            repository.Policies = JsonConvert.DeserializeObject<AzureDevOpsPolicies>(policiesResponseContent).Policies;
            return repository;
        }
    }
}
