# servicefabric-dotnetcore-webapi-angular5-msal
This sample demonstrates the use of MSAL for JavaScript for securing an Angular based single page app, implemented with an ASP.NET Web API backend hosted on Service Fabric.

---
services: active-directory, service fabric
platforms: dotnet-core
author: amigup
client: angular5
service: ASP.NET Core 2.0
endpoint: AAD V2
---

## About this sample

### Scenario

You expose a Web API and you want to protect it so that only authenticated user can access it. You want to enable authenticated users with both work and school accounts
or Microsoft personal accounts (formerly live account) to use your Web API.

An on demand video was created for the Build 2018 event, featuring this scenario and this sample. See the video [Building Web API Solutions with Authentication](https://channel9.msdn.com/Events/Build/2018/THR5000), and the associated [PowerPoint deck](http://video.ch9.ms/sessions/c1f9c808-82bc-480a-a930-b340097f6cc1/BuildWebAPISolutionswithAuthentication.pptx)

### Overview

This sample presents a Web API running on ASP.NET Core 2.0 using Azure service fabric, protected by Azure AD JWT Bearer Authentication. The Web API is exercised by a angluar5 clietn application.
The .Net application uses the Active Directory Authentication Library [MSAL.NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to obtain a JWT access token through the [OAuth 2.0](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-protocols-oauth-code) protocol. The access token is sent to the ASP.NET Core Web API, which authenticates the user using the ASP.NET JWT Bearer Authentication middleware.

Service Fabric with .NET Core
=========================================================
Check [this](https://azure.microsoft.com/en-in/resources/samples/service-fabric-dotnet-core-getting-started/) link to read about service fabric with .Net Core sample

Microsoft Authentication Library for Angular (msal-angular)
=========================================================
Check [this](https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/README.md) link to read about msal-angular

## How To Run This Sample

Getting started is simple!  To run this sample you will need:
- Visual Studio 2017
- Service Fabric
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/)
- A user account in your Azure AD tenant, or a Microsoft personal account

### Step 1:  Clone or download this repository

From your shell or command line:

```Shell
git clone https://github.com/amigup/servicefabric-dotnetcore-webapi-angular5-msal.git
```

### Step 2:  Register the sample with your Azure Active Directory tenant

There are two projects in this sample Web API and angular appliations.  Either two different appliations can be registered at Azure AD for Web API and angular applications or one application to have both Web and Web API platforms. Following steps registers only one application. To register these projects, you can:

#### Navigate to the Application registration portal

Sign in to [application registration portal](apps.dev.microsoft.com/). From there, you can add converged applications.

#### Register the angular app

1. In the [application registration portal](apps.dev.microsoft.com), click **Add an app**
1. In the *Register your application* page, provide a name for your application for instance like `servicefabric-dotnetcore-webapi-angular5-msal`
1. Press the **Create** button
1. In the registration page for your application, copy the *application ID* to the clipboard you will need it to configure the code for your application
1. Press the **Save** button at the bottom of the page.
1. In the *Platforms* section, click on the **Add Platform** button and then on **Web**
1. Click on the My applications link at the top of the page to get back to the list of applications in the app registration portal

#### Register the web API

1. In the [application registration portal](apps.dev.microsoft.com), open the registered applcation (created earlier)
1. In the *Platforms* section, click on the **Add Platform** button and then on **Web API**
1. Copy the scope proposed by default to access your web api as a user. It's in the form ``api://<Application ID>/access_as_user``
1. Press the **Save** button at the bottom of the page.

### Step 3:  Configure the sample to use your Azure AD tenant

#### Choose which users account to sign in

By default the sample is configured to enable users to sign in with any work and school accounts (AAD) or Microsoft Personal accounts (formerly live account).

##### Important note

`common` is **not** a proper tenant. It's just a **convention** to express that the accepted tenants are any Work and School organizations, or Personal Microsoft account (consumer accounts).
Accepted tenants can have the following values:

Value | Meaning
----- | --------
`common` | users can sign in with any Work and School account, or Microsoft Personal account
`organizations` |  users can sign in with any Work and School account
`consumers` |  users can sign in with a Microsoft Personal account
a GUID or domain name | users can only sign in with an account for a specific organization described by its tenant ID (GUID) or domain name
 
#### Configure the WebAPI C# project

1. Open the solution in Visual Studio.
1. In the *SampleUserService* project, open the `appsettings.json` file.
1. You can use a self-signed certificate on your local machine and test clusters but you want to make sure to purchase a CA-signed one for your production clusters.  Find the `HttpsCertificateThumbprint` property and replace with obtatined certificate thumbprint .The Service Fabric SDK provides the CertSetup.ps1 script, which creates a self-signed certificate and imports it into the Cert:\LocalMachine\My certificate store. Open a command prompt as administrator and run the following command to create a cert with the subject "CN=localhost":
```Shell
PS C:\program files\microsoft sdks\service fabric\clustersetup\secure> .\CertSetup.ps1 -Install -CertSubjectName CN=localhost
```
If you already have a certificate PFX file, run the following to import the certificate into the ``Cert:\LocalMachine\My`` certificate store:

```Shell
PS C:\mycertificates> Import-PfxCertificate -FilePath .\mysslcertificate.pfx -CertStoreLocation Cert:\LocalMachine\My -Password (ConvertTo-SecureString "!Passw0rd321" -AsPlainText -Force)


   PSParentPath: Microsoft.PowerShell.Security\Certificate::LocalMachine\My

Thumbprint                                Subject
----------                                -------
3B138D84C077C292579BA35E4410634E164075CD  CN=zwin7fh14scd.westus.cloudapp.azure.com
```
1. Find the `ClientId` property and replace the value with the Application ID (Client ID) property of the *regiseterd* application, that you registered earlier.
1. [Optional] if you want to limit sign-in to users in your organization, also update the following
- The `Domain` property, replacing the existing value with your AAD tenant domain, for example, contoso.onmicrosoft.com.
- The `TenantId` property replacing the existing value with the Tenant ID.

#### Configure the Angular project

1. In the MSALAngularDemoApp project, open `environment.ts`.
1. Find the app key `clientId` and replace the value with the ApplicationID (Client ID) for the *registered* app copied from the app registration page.
1. Find the app key `scope` and replace the value with the scope of the registered application copied from the app registration (of the form ``api://<Application ID of service>/access_as_user``)

### Step 4: Run the sample

#### Configure the WebAPI C# project
Clean the solution, rebuild the solution, and run it.  .

Once the Web API is started, Navigate to ``https://localhost/``

#### Configure the Angular project
1. Run ``npm install`` to intall all dependent packages
1. Run ``ng serve`` for a dev server. Navigate to http://localhost:4200/. The app will automatically reload if you change any of the source files.
