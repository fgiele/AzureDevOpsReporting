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
    public class Client : IClient
    {
        private const string CONTINUATIONHEADER = "x-ms-continuationtoken";
        private int projectsDoneCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="httpClient">HttpClient static reference.</param>
        public Client(HttpClient httpClient)
        {
            this.RestClient = httpClient;
        }

        /// <summary>
        /// Gets httpClient used for rest calls.
        /// </summary>
        public HttpClient RestClient { get; private set; }

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

        private static async Task<T> GetTypedAsync<T>(HttpClient restClient, string restUrl)
        {
            var restUri = new Uri(restUrl);
            var retry = 0;
            HttpResponseMessage httpResponse;
            var statusCode = System.Net.HttpStatusCode.Unused;
            var statusMessage = string.Empty;
            HttpRequestException innerException = null;

            while (retry < 3)
            {
                try
                {
                    httpResponse = await restClient.GetAsync(restUri).ConfigureAwait(false);

                    statusCode = httpResponse.StatusCode;
                    statusMessage = httpResponse.ReasonPhrase;

                    if (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var returnValue = JsonConvert.DeserializeObject<T>(responseContent, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        return returnValue;
                    }
                    else if (statusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }
                }
                catch (HttpRequestException ex)
                {
                    innerException = ex;
                }

                await Task.Delay(1000).ConfigureAwait(false);
                retry++;
            }

            throw new HttpRequestException($"Unable to request {restUri}, statuscode: {statusCode}, message: {statusMessage}", innerException);
        }

        private static async Task<List<T>> GetListTypedAsync<T>(HttpClient restClient, string restUrl)
        {
            var restUri = new Uri(restUrl);
            var retry = 0;
            HttpResponseMessage httpResponse;
            var statusCode = System.Net.HttpStatusCode.OK;
            var statusMessage = string.Empty;
            HttpRequestException innerException = null;
            var resultValues = new List<T>();

            while (retry < 3)
            {
                try
                {
                    while (statusCode == System.Net.HttpStatusCode.OK)
                    {
                        httpResponse = await restClient.GetAsync(restUri).ConfigureAwait(false);

                        statusCode = httpResponse.StatusCode;
                        statusMessage = httpResponse.ReasonPhrase;

                        if (statusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            return null;
                        }
                        else if (statusCode != System.Net.HttpStatusCode.OK)
                        {
                            break;
                        }

                        var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                        resultValues.Add(JsonConvert.DeserializeObject<T>(responseContent, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                        if (httpResponse.Headers.Contains(CONTINUATIONHEADER))
                        {
                            var continuationtoken = httpResponse.Headers.GetValues(CONTINUATIONHEADER).FirstOrDefault();
                            var uriBuilder = new UriBuilder(restUrl);
                            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                            query["continuationToken"] = continuationtoken;
                            uriBuilder.Query = query.ToString();
                            restUri = uriBuilder.Uri;
                        }
                        else
                        {
                            return resultValues;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    innerException = ex;
                }

                await Task.Delay(1000).ConfigureAwait(false);
                retry++;
            }

            throw new HttpRequestException($"Unable to request {restUri}, statuscode: {statusCode}, message: {statusMessage}", innerException);
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
            var projectResults = await GetListTypedAsync<AzureDevOpsProjects>(this.RestClient, $"{azureDevOpsUrl}{collection}/_apis/projects").ConfigureAwait(false);

            var projectResult = new AzureDevOpsProjects
            {
                Count = projectResults.Sum(prr => prr.Count),
                Projects = projectResults.SelectMany(prr => prr.Projects),
            };

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
            var releaseDefinitionScanTasks = this.ScanReleaseDefinitionsAsync(dataOptions, projectUrl);
            var repositoryScanTask = this.ScanRepositoriesAsync(dataOptions, projectUrl);

            if (buildScanTasks != null)
            {
                project.Builds = await buildScanTasks.ConfigureAwait(false);
            }

            if (releaseScanTasks != null)
            {
                project.Releases = await releaseScanTasks.ConfigureAwait(false);
            }

            if (releaseScanTasks != null)
            {
                project.ReleaseDefinitions = await releaseDefinitionScanTasks.ConfigureAwait(false);
            }

            if (repositoryScanTask != null)
            {
                project.Repositories = await repositoryScanTask.ConfigureAwait(false);
            }

            this.projectsDoneCount++;
            Console.Write(this.projectsDoneCount % 10 == 0 ? "*" : ".");

            return project;
        }

        private async Task<IEnumerable<AzureDevOpsReleaseDefinition>> ScanReleaseDefinitionsAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!dataOptions.HasFlag(DataOptions.ReleaseDefinitions))
            {
                return null;
            }

            var releaseDefResults = await GetListTypedAsync<AzureDevOpsReleaseDefinitions>(this.RestClient, $"{projectUrl}/_apis/release/definitions").ConfigureAwait(false);

            var releaseDefResult = new AzureDevOpsReleaseDefinitions
            {
                Count = releaseDefResults.Sum(rdr => rdr.Count),
                ReleaseDefinitions = releaseDefResults.SelectMany(rdr => rdr.ReleaseDefinitions),
            };

            var releaseDefinitions = Array.Empty<AzureDevOpsReleaseDefinition>();
            if (releaseDefResult.Count > 0)
            {
                var releaseDefinitionScanTasks = new HashSet<Task<AzureDevOpsReleaseDefinition>>();
                var releaseDefinitionUrls = releaseDefResult.ReleaseDefinitions.Select(rdef => rdef.Url).ToArray();

                foreach (var releaseDefUrl in releaseDefinitionUrls)
                {
                    releaseDefinitionScanTasks.Add(this.ScanReleaseDefinitionAsync(releaseDefUrl.AbsoluteUri));
                }

                releaseDefinitions = await Task.WhenAll(releaseDefinitionScanTasks).ConfigureAwait(false);
            }

            return releaseDefinitions;
        }

        private async Task<AzureDevOpsReleaseDefinition> ScanReleaseDefinitionAsync(string releaseDefinitionUrl)
        {
            return await GetTypedAsync<AzureDevOpsReleaseDefinition>(this.RestClient, releaseDefinitionUrl).ConfigureAwait(false);
        }

        private async Task<IEnumerable<AzureDevOpsBuild>> ScanBuildsAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Build) ||
                dataOptions.HasFlag(DataOptions.BuildArtifacts)))
            {
                return null;
            }

            var buildresults = await GetListTypedAsync<AzureDevOpsBuilds>(this.RestClient, $"{projectUrl}/_apis/build/builds").ConfigureAwait(false);

            var buildresult = new AzureDevOpsBuilds
            {
                Count = buildresults.Sum(bur => bur.Count),
                Builds = buildresults.SelectMany(bur => bur.Builds),
            };

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
            build.Artifacts = (await GetTypedAsync<AzureDevOpsBuildArtifacts>(this.RestClient, $"{build.Url}/artifacts?api-version=5.1").ConfigureAwait(false)).Artifacts;
            build.Timeline = await GetTypedAsync<AzureDevOpsBuildTimeline>(this.RestClient, $"{build.Url}/timeline").ConfigureAwait(false);
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
            var releaseresults = await GetListTypedAsync<AzureDevOpsReleases>(this.RestClient, $"{projectUrl}/_apis/release/releases").ConfigureAwait(false);

            var releaseresult = new AzureDevOpsReleases
            {
                Count = releaseresults.Sum(rer => rer.Count),
                Releases = releaseresults.SelectMany(rer => rer.Releases),
            };

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
            return await GetTypedAsync<AzureDevOpsRelease>(this.RestClient, $"{release.Url}").ConfigureAwait(false);
        }

        private async Task<IEnumerable<AzureDevOpsRepository>> ScanRepositoriesAsync(DataOptions dataOptions, string projectUrl)
        {
            if (!(dataOptions.HasFlag(DataOptions.Git) ||
                dataOptions.HasFlag(DataOptions.GitPolicies)))
            {
                return null;
            }

            var repositoryResults = await GetListTypedAsync<AzureDevOpsRepositories>(this.RestClient, $"{projectUrl}/_apis/git/repositories?api-version=5.0").ConfigureAwait(false);

            var repositoryResult = new AzureDevOpsRepositories
            {
                Count = repositoryResults.Sum(rer => rer.Count),
                Repositories = repositoryResults.SelectMany(rer => rer.Repositories),
            };

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
            repository.Policies = (await GetTypedAsync<AzureDevOpsPolicies>(this.RestClient, $"{projectUrl}/_apis/git/policy/configurations?repositoryId={repository.Id}&refName={branchName}").ConfigureAwait(false)).Policies;
            return repository;
        }
    }
}
