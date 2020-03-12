# Compliance reporting
This project is used to generate CSV (Excel) reports about the use of Build and Release pipelines on Azure DevOps.

## Introduction
Proper use of a software delivery pipeline includes compliance to the minimum standards set by an organisation with regards to software development. These might include the mandatory use of code reviews, approval processes and even mandatory actions to be taken during the deployment process on specific environments.
Since a process is only as useful as compliance to the workflow, this software enables scanning of Azure DevOps to determine what options are being used, so this can be compared to required minimum standards.

## Software design
The code is build in .NET Core, using the REST API of Azure DevOps to enable connectivity to both on-premise and cloud services. All information is collected in an in-memory model, after which a report is generated. The only dependencies this software has is an operating system capable of running .NET Core and network connectivity to the environment to be monitored.

[![Build Status](https://dev.azure.com/fgi/AzureDevOps/_apis/build/status/fgiele.AzureDevOpsReporting?branchName=master)](https://dev.azure.com/fgi/AzureDevOps/_build/latest?definitionId=5&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ComplianceReporting&metric=alert_status)](https://sonarcloud.io/dashboard?id=ComplianceReporting)