to run the app, create an appsettings.json file with the following configuration:

```{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "ConnectionStrings" : {
    "DefaultConnection" : "Your-absolutely-majestic-connection-string"
  },
  "Jwt" : {
      "Issuer" : "http://localhost:5300",
      "Audience" : "http://localhost:5300",
      "Key" : "AbsolutelySecretKEy",
      "ValidInMinutes" : 10
  }
}
```
