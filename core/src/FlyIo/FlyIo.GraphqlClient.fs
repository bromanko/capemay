namespace FlyIo

open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Fable.Remoting.Json
open System
open System.Net.Http
open System.Text
open System.IO
open FSharp.Control.Tasks

type GraphqlInput<'T> = { query: string; variables: Option<'T> }
type GraphqlSuccessResponse<'T> = { data: 'T }
type GraphqlErrorResponse = { errors: FlyIoError list }

type FlyIoGraphqlClient private (httpClient: HttpClient, url: string) =

    let fableJsonConverter = FableJsonConverter() :> JsonConverter
    let settings = JsonSerializerSettings(DateParseHandling=DateParseHandling.None, Converters = [| fableJsonConverter |])
    let serializer = JsonSerializer.Create(settings)

    /// <summary>Creates FlyIoGraphqlClient specifying <see href="T:System.Net.Http.HttpClient">HttpClient</see> instance</summary>
    /// <remarks>
    /// In order to enable all F# types serialization and deserealization
    /// <see href="T:Fable.Remoting.Json.FableJsonConverter">FableJsonConverter</see> is added
    /// from <a href="https://github.com/Zaid-Ajaj/Fable.Remoting">Fable.Remoting.Json</a> NuGet package
    /// </remarks>
    /// <param name="url">GraphQL endpoint URL</param>
    /// <param name="httpClient">The HttpClient to use for issuing the HTTP requests</param>
    new(url: string, httpClient: HttpClient) = FlyIoGraphqlClient(httpClient, url)

    /// <summary>Creates FlyIoGraphqlClient with a new <see href="T:System.Net.Http.HttpClient">HttpClient</see> instance</summary>
    /// <remarks>
    /// In order to enable all F# types serialization and deserealization
    /// <see href="T:Fable.Remoting.Json.FableJsonConverter">FableJsonConverter</see> is added
    /// from <a href="https://github.com/Zaid-Ajaj/Fable.Remoting">Fable.Remoting.Json</a> NuGet package
    /// </remarks>
    /// <param name="url">GraphQL endpoint URL</param>
    new(url: string) = FlyIoGraphqlClient(url, new HttpClient())

    /// <summary>Creates FlyIoGraphqlClient specifying <see href="T:System.Net.Http.HttpClient">HttpClient</see> instance</summary>
    /// <remarks>
    /// In order to enable all F# types serialization and deserealization
    /// <see href="T:Fable.Remoting.Json.FableJsonConverter">FableJsonConverter</see> is added
    /// from <a href="https://github.com/Zaid-Ajaj/Fable.Remoting">Fable.Remoting.Json</a> NuGet package
    /// </remarks>
    /// <param name="httpClient">The HttpClient to use for issuing the HTTP requests</param>
    new(httpClient: HttpClient) =
        if httpClient.BaseAddress <> null then
            FlyIoGraphqlClient(httpClient.BaseAddress.OriginalString, httpClient)
        else
            raise(ArgumentNullException("BaseAddress of the HttpClient cannot be null for the constructor that only accepts a HttpClient"))
            FlyIoGraphqlClient(String.Empty, httpClient)
    
    member _.LaunchMachineAsync(input: LaunchMachine.InputVariables) =
        task {
            let query = """
                mutation ($input: LaunchMachineInput!) {
                  launchMachine(input: $input) {
                    machine {
                      id
                      name
                    }
                  }
                }
            """
            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, settings)
            let! response = httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStreamAsync())
            use sr = new StreamReader(responseContent)
            use tr = new JsonTextReader(sr)
            let responseJson = serializer.Deserialize<JObject>(tr)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<LaunchMachine.Query>>(serializer)
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                return Error response.errors
        }

    member this.LaunchMachine(input: LaunchMachine.InputVariables) = Async.RunSynchronously(Async.AwaitTask(this.LaunchMachineAsync input))


    member _.CreateVolumeAsync(input: CreateVolume.InputVariables) =
        task {
            let query = """
                mutation ($input: CreateVolumeInput!) {
                  createVolume(input: $input) {
                    volume {
                      id
                      name
                    }
                  }
                }
            """
            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, settings)
            let! response = httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStreamAsync())
            use sr = new StreamReader(responseContent)
            use tr = new JsonTextReader(sr)
            let responseJson = serializer.Deserialize<JObject>(tr)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<CreateVolume.Query>>(serializer)
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                return Error response.errors
        }

    member this.CreateVolume(input: CreateVolume.InputVariables) = Async.RunSynchronously(Async.AwaitTask(this.CreateVolumeAsync input))


    member _.CreateAppAsync(input: CreateApp.InputVariables) =
        task {
            let query = """
                mutation ($input: CreateAppInput!) {
                  createApp(input: $input) {
                    app {
                      id
                      name
                    }
                  }
                }
            """
            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, settings)
            let! response = httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStreamAsync())
            use sr = new StreamReader(responseContent)
            use tr = new JsonTextReader(sr)
            let responseJson = serializer.Deserialize<JObject>(tr)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<CreateApp.Query>>(serializer)
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                return Error response.errors
        }

    member this.CreateApp(input: CreateApp.InputVariables) = Async.RunSynchronously(Async.AwaitTask(this.CreateAppAsync input))


    member _.GetAppAsync(input: GetApp.InputVariables) =
        task {
            let query = """
                query ($appName: String!) {
                  app(name: $appName) {
                    appUrl
                    createdAt
                    deployed
                    hostname
                    id
                    name
                    organization {
                      id
                      name
                    }
                    regions {
                      name
                      code
                    }
                    status
                    machines {
                      nodes {
                        name
                        id
                        ips {
                          nodes {
                            id
                            ip
                            kind
                            family
                          }
                        }
                      }
                    }
                    volumes {
                      nodes {
                        id
                        name
                        sizeGb
                        state
                        status
                        usedBytes
                      }
                    }
                  }
                }
            """
            let inputJson = JsonConvert.SerializeObject({ query = query; variables = Some input }, settings)
            let! response = httpClient.PostAsync(url, new StringContent(inputJson, Encoding.UTF8, "application/json"))

            let! responseContent = Async.AwaitTask(response.Content.ReadAsStreamAsync())
            use sr = new StreamReader(responseContent)
            use tr = new JsonTextReader(sr)
            let responseJson = serializer.Deserialize<JObject>(tr)

            match response.IsSuccessStatusCode with
            | true ->
                let errorsReturned =
                    responseJson.ContainsKey "errors"
                    && responseJson.["errors"].Type = JTokenType.Array
                    && responseJson.["errors"].HasValues

                if errorsReturned then
                    let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                    return Error response.errors
                else
                    let response = responseJson.ToObject<GraphqlSuccessResponse<GetApp.Query>>(serializer)
                    return Ok response.data

            | errorStatus ->
                let response = responseJson.ToObject<GraphqlErrorResponse>(serializer)
                return Error response.errors
        }

    member this.GetApp(input: GetApp.InputVariables) = Async.RunSynchronously(Async.AwaitTask(this.GetAppAsync input))
