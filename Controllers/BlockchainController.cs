using IQC_WebClient_Workbench.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebClientWorkbench.Extensions;
using WebClientWorkbench.Models;
using WebClientWorkbench.Utils;
using System.Numerics;


namespace WebClientWorkbench.Controllers
{
    public class BlockchainController : Controller
    {
        public async Task<IActionResult> Index()
        {
            AuthenticationResult result = null;

            try
            {
                // Because we signed-in already in the WebApp, the userObjectId is know
                string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;

                // Using ADAL.Net, get a bearer token to access the WorkbenchListService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, 
					new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.WorkbenchResourceId, 
					credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, 
					AzureAdOptions.Settings.WorkbenchBaseAddress + "/api/v1/applications");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String json_string = await response.Content.ReadAsStringAsync();
                    ApplicationReturnType applicationsResponse = JsonConvert.DeserializeObject<ApplicationReturnType>(json_string);

					List<IQC_WebClient_Workbench.Models.Application> applications = applicationsResponse.Applications;
                    return View(applications);
                }
                else
                {
                    var x = 0;
                }
            }
            catch (Exception ee)
            {
                if (HttpContext.Request.Query["reauth"] == "True" || ee.Message.ToLower().Contains("silently"))
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
                }

                return View(new List<IQC_WebClient_Workbench.Models.Application>() { new IQC_WebClient_Workbench.Models.Application() { Name = ee.InnerException.Message } });
            }
            //
            // If the call failed for any other reason, show the user an error.
            //
            return View(new List<IQC_WebClient_Workbench.Models.Application>() { new IQC_WebClient_Workbench.Models.Application() { Name = "Error!!" } });
        }

        public async Task<IActionResult> Workflows()
		{
            AuthenticationResult result = null;
            try
			{
				// Because we signed-in already in the WebApp, the userObjectId is know
				string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;

				// Using ADAL.Net, get a bearer token to access the WorkbenchListService
				AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, 
					new NaiveSessionCache(userObjectID, HttpContext.Session));
				ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, 
					AzureAdOptions.Settings.ClientSecret);

				result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.WorkbenchResourceId, credential, 
					new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, 
					AzureAdOptions.Settings.WorkbenchBaseAddress + "/api/v1/applications/1/workflows");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
				HttpResponseMessage response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{

					JsonSerializerSettings settings = new JsonSerializerSettings();
					String json_string = await response.Content.ReadAsStringAsync();
					WorkflowReturnType workflowResponse = JsonConvert.DeserializeObject<WorkflowReturnType>(json_string);

					List<IQC_WebClient_Workbench.Models.Workflow> workflows = workflowResponse.Workflows;


					return View(workflows);
				}
				else
				{
					var x = 0;
				}
			}
            catch (Exception ee)
            {
                if (HttpContext.Request.Query["reauth"] == "True" || ee.Message.ToLower().Contains("silently"))
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
                }

                return  View(new List<IQC_WebClient_Workbench.Models.Workflow>() { new IQC_WebClient_Workbench.Models.Workflow() { Name = ee.InnerException.Message } });
            }

            return View(new List<IQC_WebClient_Workbench.Models.Workflow>() { new IQC_WebClient_Workbench.Models.Workflow() { Name = "Error" } });
        }

        public async Task<IActionResult> Contracts()
        {
            AuthenticationResult result = null;
            try
            {
                // Because we signed-in already in the WebApp, the userObjectId is know
                string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;

                // Using ADAL.Net, get a bearer token to access the WorkbenchListService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, 
					new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.WorkbenchResourceId, credential, 
					new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, 
					AzureAdOptions.Settings.WorkbenchBaseAddress + "/api/v1/contracts?workflowId=1");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String json_string = await response.Content.ReadAsStringAsync();
                    WorkflowInstancesReturnType workflowistancesResponse = JsonConvert.DeserializeObject<WorkflowInstancesReturnType>(json_string);

                    List<IQC_WebClient_Workbench.Models.Contract> contracts = workflowistancesResponse.Contracts;
                    return View(contracts);
                }
                else
                {
                    var x = 0;
                }
            }
            catch (Exception ee)
            {
                if (HttpContext.Request.Query["reauth"] == "True" || ee.Message.ToLower().Contains("silently"))
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
                }

                return View(new List<IQC_WebClient_Workbench.Models.Workflow>() { new IQC_WebClient_Workbench.Models.Workflow() { Name = ee.InnerException.Message } });
            }

            return View(new List<IQC_WebClient_Workbench.Models.Workflow>() { new IQC_WebClient_Workbench.Models.Workflow() { Name = "Error" } });
        }
        [HttpGet]
        public ActionResult NewContract()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewContract(NewContractViewModel model)
        {
            List<ContractActionParameter> paramList = new List<ContractActionParameter>();
            paramList.Add(new ContractActionParameter { Name = "Device", Value = model.Device });
			paramList.Add(new ContractActionParameter { Name = "SupplyChainOwner", Value = model.Owner });
			paramList.Add(new ContractActionParameter { Name = "SupplyChainObserver", Value = model.Observer });
			paramList.Add(new ContractActionParameter { Name = "MinHumidity", Value = model.MinHumidity.ToString() });
			paramList.Add(new ContractActionParameter { Name = "MaxHumidity", Value = model.MaxHumidity.ToString() });
			paramList.Add(new ContractActionParameter { Name = "MinTemperature", Value = model.MinTemperature.ToString() });
			paramList.Add(new ContractActionParameter { Name = "MaxTemperature", Value = model.MaxTemperature.ToString() });

			WorkflowActionInput wfActionInput = new WorkflowActionInput()
            {
                WorkflowActionParameters = paramList,
                WorkflowFunctionID = 1
            };

            var jsonObject = JsonConvert.SerializeObject(wfActionInput);

            AuthenticationResult result = null;
            try
            {
                // Because we signed-in already in the WebApp, the userObjectId is know
                string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;

                // Using ADAL.Net, get a bearer token to access the WorkbenchListService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, 
					new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.WorkbenchResourceId, credential,
					new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(AzureAdOptions.Settings.WorkbenchBaseAddress);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, 
					"/api/v1/contracts?workflowId=1&contractCodeId=1&connectionId=1");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                request.Content = new StringContent(jsonObject.ToString(), System.Text.Encoding.UTF8, "application/json");
 
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String json_string = await response.Content.ReadAsStringAsync();
                    int newContractID = JsonConvert.DeserializeObject<int>(json_string);

                    HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.WorkbenchBaseAddress + "/api/v1/contracts/" + newContractID);
                    newRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    HttpResponseMessage newResponse = await client.SendAsync(newRequest);

                    if (newResponse.IsSuccessStatusCode)
                    {
                        String new_json_string = await newResponse.Content.ReadAsStringAsync();
                        Contract newContract = JsonConvert.DeserializeObject<Contract>(new_json_string);

                        return RedirectToAction("ContractDetail", newContract);
                    }
                    else
                    {
                        var x = 0;
                    }
                    return View();
                }
                else
                {
                    var x = 0;
                }
            }
            catch (Exception e)
            {

                throw;
            }
            return View();
        }

        public ActionResult ContractDetail(Contract c)
        {
            return View(c);
        }



		//add
		[HttpGet]
		public async Task<JsonResult> Users(string role)
		{
			AuthenticationResult result = null;

			try
			{
				// Because we signed-in already in the WebApp, the userObjectId is know
				string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;

				// Using ADAL.Net, get a bearer token to access the WorkbenchListService
				AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority,
					new NaiveSessionCache(userObjectID, HttpContext.Session));
				ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
				result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.WorkbenchResourceId,
					credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));


				HttpClient client = new HttpClient();

				#region User role의  roleid 탐색 시작 ----------------------------------------------------------------

				var roleId = 0L;

				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
					AzureAdOptions.Settings.WorkbenchBaseAddress + "/api/v1/applications/1");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
				HttpResponseMessage response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{					
					String json_string = await response.Content.ReadAsStringAsync();
					Application applicationsResponse = JsonConvert.DeserializeObject<Application>(json_string);
					List<IQC_WebClient_Workbench.Models.ApplicationRole> applications = applicationsResponse.ApplicationRoles;
					roleId = applications.SingleOrDefault(x => x.Name == role).Id;					
				}
				#endregion User role의  roleid 탐색 끝 ----------------------------------------------------------------


				//해당 롤의 사용자 검색
				request = new HttpRequestMessage(HttpMethod.Get,
					AzureAdOptions.Settings.WorkbenchBaseAddress + $"/api/v1/applications/1/roleAssignments?applicationRoleId={roleId}");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
				response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					JsonSerializerSettings settings = new JsonSerializerSettings();
					String json_string = await response.Content.ReadAsStringAsync();
					RoleAssignmentReturnType roleAssignmentResponse = JsonConvert.DeserializeObject<RoleAssignmentReturnType>(json_string);

					List<IQC_WebClient_Workbench.Models.RoleAssignment> roleAssignments = roleAssignmentResponse.RoleAssignments;
					
					var jsonResult = Json(roleAssignments);					
					return jsonResult;
				}
				else
				{
					var x = 0;
				}
			}
			catch (Exception ee)
			{
				if (HttpContext.Request.Query["reauth"] == "True" || ee.Message.ToLower().Contains("silently"))
				{
					return Json(ee.Message);
				}

				return Json(ee.Message);
			}
			return Json("error");
		}

	}
}