// -----------------------------------------------------------------------
// <copyright file="Client.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AzureDevOps.Model;
    using Newtonsoft.Json;

    /// <summary>
    /// Scanning client for AzureDevOps instance. Handles collection of data.
    /// </summary>
    public class Client
    {
        private readonly HttpClient restClient;

        private int projectsDoneCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="httpClient">HttpClient static reference.</param>
        public Client(HttpClient httpClient)
        {
            this.restClient = httpClient;
        }

        /// <summary>
        /// Starts the scanning process of the Azure DevOps instance.
        /// </summary>
        /// <param name="dataOptions">Scanning options, determines the level and subjects of scanning.</param>
        /// <param name="collections">List of collections to be scanned.</param>
        /// <param name="azureDevOpsUrl">Uri of the Azure DevOps instance to be scanned.</param>
        /// <returns>Instance object holding all collected data.</returns>
        public Task<AzureDevOpsInstance> ScanAsync(DataOptions dataOptions, IEnumerable<string> collections, Uri azureDevOpsUrl)
        {
            if (collections == null)
            {
                throw new ArgumentNullException(nameof(collections));
            }

            if (azureDevOpsUrl == null)
            {
                throw new ArgumentNullException(nameof(azureDevOpsUrl));
            }

            return this.ScanAzDOAsync(dataOptions, collections, azureDevOpsUrl);
        }

        private async Task<AzureDevOpsInstance> ScanAzDOAsync(DataOptions dataOptions, IEnumerable<string> collections, Uri azureDevOpsUrl)
        {
            this.projectsDoneCount = 0;

            var azureDevOpsInstance = new AzureDevOpsInstance();

            if (azureDevOpsUrl.AbsoluteUri.StartsWith("https://dev.azure.com", StringComparison.OrdinalIgnoreCase))
            {
                // There should be only one Org, since a PAT is limited to Organisation
                var organisation = collections.Single();

                Console.WriteLine($"Scanning organisation: [{organisation}]");
                azureDevOpsInstance.Collections.Add(await this.ScanCollectionAsync(dataOptions, organisation, azureDevOpsUrl.AbsoluteUri).ConfigureAwait(false));
            }
            else
            {
                var collectionTasks = new HashSet<Task<AzureDevOpsCollection>>();

                foreach (var collection in collections)
                {
                    Console.WriteLine($"Scanning collection: [{collection}]");
                    collectionTasks.Add(this.ScanCollectionAsync(dataOptions, collection, azureDevOpsUrl.AbsoluteUri));
                }

                azureDevOpsInstance.Collections.AddRange(await Task.WhenAll(collectionTasks).ConfigureAwait(false));
            }

            return azureDevOpsInstance;
        }

        private async Task<AzureDevOpsCollection> ScanCollectionAsync(DataOptions dataOptions, string collection, string azureDevOpsUrl)
        {
            HttpResponseMessage httpProjectResponse = await this.restClient.GetAsync(new Uri($"{azureDevOpsUrl}{collection}/_apis/projects")).ConfigureAwait(false);
            var responseProjectContent = await httpProjectResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var projectResult = JsonConvert.DeserializeObject<AzureDevOpsProjects>(responseProjectContent);

            var azDOCollection = new AzureDevOpsCollection { Name = collection };
            var scanProjectTasks = new HashSet<Task<AzureDevOpsProject>>();

            Console.WriteLine($"Scanning [{projectResult.Count}] projects");

            foreach (var project in projectResult.Projects)
            {
                scanProjectTasks.Add(this.ScanProjectAsync(dataOptions, $"{azureDevOpsUrl}{collection}/{project.Id}", project));
            }

            azDOCollection.Projects.AddRange(await Task.WhenAll(scanProjectTasks).ConfigureAwait(false));

            Console.WriteLine();

            return azDOCollection;
        }

        private async Task<AzureDevOpsProject> ScanProjectAsync(DataOptions dataOptions, string projectUrl, AzureDevOpsProject project)
        {
            var buildScanTasks = this.ScanBuildsAsync(dataOptions, projectUrl);
            var releaseScanTasks = this.ScanReleasesAsync(dataOptions, projectUrl);
            var repositoryScanTask = this.ScanRepositoriesAsync(dataOptions, projectUrl);

            if (buildScanTasks != null)
            {
                project.Builds = await buildScanTasks.ConfigureAwait(false);
            }

            if (releaseScanTasks != null)
            {
                project.Releases = await releaseScanTasks.ConfigureAwait(false);
            }

            if (repositoryScanTask != null)
            {
                project.Repositories = await repositoryScanTask.ConfigureAwait(false);
            }

            this.projectsDoneCount++;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Console.Write(this.projectsDoneCount % 10 == 0 ? "*" : ".");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

            return project;
        }

        private async Task<IEnumerable<AzureDevOpsBuild>> ScanBuildsAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Build) ||
                dataOptions.HasFlag(DataOptions.BuildArtifacts)))
            {
                return null;
            }

            HttpResponseMessage httpBuildResponse = await this.restClient.GetAsync(new Uri($"{projectUrl}/_apis/build/builds")).ConfigureAwait(false);
            var responseBuildContent = await httpBuildResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                        buildScanTasks.Add(this.ScanBuildAsync(build));
                    }

                    builds = await Task.WhenAll(buildScanTasks).ConfigureAwait(false);
                }
            }

            return builds;
        }

        private async Task<AzureDevOpsBuild> ScanBuildAsync(AzureDevOpsBuild build)
        {
            HttpResponseMessage httpArtifactResponse = await this.restClient.GetAsync(new Uri($"{build.Url}/artifacts?api-version=5.1")).ConfigureAwait(false);
            var artifactResponseContent = await httpArtifactResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            build.Artifacts = JsonConvert.DeserializeObject<AzureDevOpsBuildArtifacts>(artifactResponseContent).Artifacts;
            return build;
        }

        private async Task<IEnumerable<AzureDevOpsRelease>> ScanReleasesAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Release) ||
                dataOptions.HasFlag(DataOptions.ReleaseDetails)))
            {
                return null;
            }

            // Azure DevOps services uses a different host for release management Rest calls
            projectUrl = projectUrl.Replace("https://dev.azure.com", "https://vsrm.dev.azure.com", StringComparison.OrdinalIgnoreCase);
            HttpResponseMessage httpReleaseResponse = await this.restClient.GetAsync(new Uri($"{projectUrl}/_apis/release/releases")).ConfigureAwait(false);
            var responseReleaseContent = await httpReleaseResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releaseresult = JsonConvert.DeserializeObject<AzureDevOpsReleases>(responseReleaseContent);

            var releases = Array.Empty<AzureDevOpsRelease>();
            if (releaseresult.Count > 0)
            {
                releases = releaseresult.Releases.ToArray();
                if (dataOptions.HasFlag(DataOptions.ReleaseDetails))
                {
                    var releaseScanTasks = new HashSet<Task<AzureDevOpsRelease>>();
                    foreach (var release in releases)
                    {
                        releaseScanTasks.Add(this.ScanReleaseDetailAsync(release));
                    }

                    releases = await Task.WhenAll(releaseScanTasks).ConfigureAwait(false);
                }
            }

            return releases;
        }

        private async Task<AzureDevOpsRelease> ScanReleaseDetailAsync(AzureDevOpsRelease release)
        {
            HttpResponseMessage httpReleaseDetailResponse = await this.restClient.GetAsync(new Uri($"{release.Url}")).ConfigureAwait(false);
            var releaseDetailResponseContent = await httpReleaseDetailResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<AzureDevOpsRelease>(releaseDetailResponseContent);
        }

        private async Task<IEnumerable<AzureDevOpsRepository>> ScanRepositoriesAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Git) ||
                dataOptions.HasFlag(DataOptions.GitPolicies)))
            {
                return null;
            }

            HttpResponseMessage httpRepositoriesResponse = await this.restClient.GetAsync(new Uri($"{projectUrl}/_apis/git/repositories?api-version=5.0")).ConfigureAwait(false);
            var policiesResponseContent = await httpRepositoriesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var repositoryResult = JsonConvert.DeserializeObject<AzureDevOpsRepositories>(policiesResponseContent);

            var repositories = Array.Empty<AzureDevOpsRepository>();
            if (repositoryResult.Count > 0)
            {
                repositories = repositoryResult.Repositories.ToArray();
                if (dataOptions.HasFlag(DataOptions.GitPolicies))
                {
                    var policyscanTasks = new HashSet<Task<AzureDevOpsRepository>>();
                    foreach (var repository in repositories)
                    {
                        policyscanTasks.Add(this.ScanBranchPoliciesAsync(projectUrl, repository));
                    }

                    repositories = await Task.WhenAll(policyscanTasks).ConfigureAwait(false);
                }
            }

            return repositories;
        }

        private async Task<AzureDevOpsRepository> ScanBranchPoliciesAsync(string projectUrl, AzureDevOpsRepository repository, string branchName = "refs/heads/master")
        {
            HttpResponseMessage httpPoliciesResponse = await this.restClient.GetAsync(new Uri($"{projectUrl}/_apis/git/policy/configurations?repositoryId={repository.Id}&refName={branchName}")).ConfigureAwait(false);
            var policiesResponseContent = await httpPoliciesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            repository.Policies = JsonConvert.DeserializeObject<AzureDevOpsPolicies>(policiesResponseContent).Policies;
            return repository;
        }
    }
}
