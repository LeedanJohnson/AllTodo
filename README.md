Take Input: (AllTodo.CLI -> CLI_account.cs Line 69-ish)
Perform Operations/calculations/rules: (AllTodo.Server -> Controllers -> TodoController.cs Lines 72-84)
Show output: (AllTodo.CLI -> CLI_todo.cs Lines 88-114)
Store Values in a database: Well.... I store all values in an in-memory database...
Log operations: Docker logs things for us! :)

Aggregates: Example is the User containing Username, Password and PhoneNumber. Those values are accessed outside the User aggregate, but cannot be changed. (AllTodo.Shared -> Models -> User.cs Line 16)
Entities: Example is the Todo class it needs an ID as it is identified by more than just the data it contains. (AllTodo.Shared -> Models -> Todo.cs Line 38)
Value Objects: The TokenCredentials class is a valueobject because it contains domain primitives that define it's value/identity. (AllTodo.Shared -> Models -> TokenCredentials.cs Line 25)
Domain Primitives: There are many domain primitives in the (AllTodo.Shared -> Models -> Primitives) folder. They are ubiquitous in the domain, and like value objects are identified by their value alone.
Appropriate Logging: WE log everything at the INFO level and below. Because we want to know everything. (and that's the default)

5 Validation levels/rules:
+ Origin of the data: We don't really check this, because we would expect this will be a public RESTful API.
+ Size of the data: 
+ Lexical: Because we use the ASP.NET Core fremework, it handles this for us.
+ Syntax: Because we use the ASP.NET Core fremework, it handles this for us.
+ Semantic: We actually do this one ourselves. We check that all required fields are present, then validate before proceeding.
    + Validating fields present (AllTodo.Server -> Controllers -> TodoController.cs lines 62-70)
    + Validating content (AllTodo.Server -> Controllers -> TodoController.cs line 78 : AllTodo.Shared -> Models -> Todo.cs lines 110-133)

Read once objects: Not sure if it counts as 'read once' but when we store the password, we hash it in the constructor, so the plain-text password is only read once. (AllTodo.Shared -> Models -> Primitives -> HashedPassword.cs line 19

Appropriate use of failures/exceptions: Example: When a user fails to be authenticated, we return a 400 response. (AllTodo.Server -> Controllers -> TodoController.cs line 74/86)

Builder pattern: We use the builder pattern / fluent contruction when setting up the server in (AllTodo.Server -> Program.cs lines 21-25 )
