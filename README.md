# PKCSwagger

> PKCSwagger

## How to use

in your .net core web api project

`Startup`

```C#
// var
private readonly KTSwaggerOptionSetting swaggerOptionSetting = new KTSwaggerOptionSetting
        {
            Title = "Services API",
            Description = "Services API",
            TermsOfService = "Services API",
            Contact = new Contact
            {
                Name = "yasuoyuhao",
                Email = "yasuoyuhao@gmail.com"
            }
        };
```

add api versioning

```C#
// in Startup ConfigureServices
services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
```

add PKCSwagger

```C#
// in Startup ConfigureServices
// KTSwagger
services.AddKTSwagger(swaggerOptionSetting);
```

use

```C#
// in Startup Configure
app.UseKTSwagger();
```

now, run your web app and open `http://your-hosting/swagger` and you can see swagger doc