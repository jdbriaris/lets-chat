# lets-chat
A simple chat application that demonstrates the Azure Service Bus

![Azure Service Bus Architecture](https://github.com/jdbriaris/lets-chat/blob/master/lets-chat-tests/Images/azure_service_bus.PNG)

## Setup
To run the application you must first create a SeviceBus namespace on Azure. Once created, add the following configuration setting to `App.config`

```xml
<appSettings>
  <!-- Service Bus specific app setings for messaging connections -->
  <add key="Microsoft.ServiceBus.ConnectionString"
            value="[YourConnectionString]"/>
</appSettings>
```
where `[YourConnectionString]` is the primary key connection string for the service bus namespace that you just created.

## Corporate Proxy
When running the application behind a corporate proxy, connections to Azure services may be refused due to a [407](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) proxy authentication error. To circumvent this add the following to `App.config`

```xml
<system.net>
  <defaultProxy useDefaultCredentials="true">
  </defaultProxy>
</system.net>
```

