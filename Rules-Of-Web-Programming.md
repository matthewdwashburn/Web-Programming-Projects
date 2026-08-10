# Rules Of Web Programming

1. **No inline styles or inline javascript in HTML files.** Three exceptions to this rule that are okay:
   - Event handlers (e.g. onClick) that call named functions
   - Setting the style attribute in React components
   - Javascript event handlers in React components

2. **Error messages must be "in-page"** i.e. no pop-ups or alerts.

3. **Any resources not created by you** (images, javascript libraries, etc.) **must be referenced using a CDN or URL**, not directly included in your assignment submission. In addition, do not include any build output folders in your submission. Examples include `node_modules`, `debug`, `release`, `bin`, and `obj`.

4. **All requests that submit a body to your server must have their entities validated** with appropriate annotations, such as `MinLength`, `Range`, or `Required`.

5. **The main page of your of your application must either be served from the server root path** or use a client app in the same directory that can be started locally with `npm`.

6. **Service/data/model classes must not have any http, request, or response references.**

7. **Controller entity classes must not be used directly to store data on the server;** translate them into a model (data storage) class before saving the data. Conversely, controllers must not send any model classes to the user; translate them into controller entity classes before sending the response.

8. **All service class instances must be obtained using dependency injection.**

9. **You may not use any synchronous methods in your C# code** wherever there is an async option.

10. **All controllers (and their corresponding entities) must enforce the usage of an api version.** Your namespace and folder structure for controllers and entities must contain the api version.
