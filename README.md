# lets-chat
A simple chat application that demonstrates the Azure Service Bus

## Corporate Proxy
When running the application behind a corporate proxy, connections to Azure services may be refused due to a [407](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) proxy authentication error. To circumvent this add the following to `App.config`

```xml
<system.net>
  <defaultProxy useDefaultCredentials="true">
  </defaultProxy>
</system.net>
```

