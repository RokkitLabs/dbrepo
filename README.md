# 🗄️ DbRepo
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/EpicTestingTempOrganizationForStuff/dbrepo/.NET%20Core?style=for-the-badge) ![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/EpicTestingTempOrganizationForStuff/dbrepo/.NET%20Core/release?label=Release%20Build&style=for-the-badge) ![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/EpicTestingTempOrganizationForStuff/dbrepo/publish%20to%20nuget/release?color=004880&label=Nuget%20Publish&style=for-the-badge)
# 👷 Installation
## Get it from Nuget
[Latest](https://nuget.org)
## Download from releases
[Latest](https://github.com/unlimitedcoder2/dbrepo/releases)

# 🕴️ Usage
## Create repo
```cs
using DbRepo;

//Create a new context
MyDbContext context = new MyDbContext();
//Get repo from context
DbRepo<User> userRepo = context.GetRepo<User>();
```

## Insert
```cs
User newUser = new User {
    Id = 123,
    Username = "Test User"
};
//Await the insertion of the newUser
await userRepo.InsertOneAsync(newUser).ConfigureAwait(false);
```

## Find One
```cs
User firstDbRepoUser = await UserRepo.FindOneAsync(new { Id = 123 }).ConfigureAwait(false);
```

# 🥅 Goals
* [ ] Usable

## ✨ Contributors

<table>
  <tr>
    <td align="center"><a href="https://ahowe.dev/"><img src="https://avatars2.githubusercontent.com/u/16884313?v=4" width="100px;" alt=""/><br /><sub><b>unlimitedcoder2</b></sub></a><br /><a href="https://github.com/unlimitedcoder2/dbrepo/commits?author=unlimitedcoder2" title="Code">💻</a></td>
       <td align="center"><a href="https://mwareing.xyz/"><img src="https://avatars1.githubusercontent.com/u/29664925?s=460&v=4" width="100px;" alt=""/><br /><sub><b>TatoExp</b></sub></a><br /><a href="https://github.com/unlimitedcoder2/dbrepo/commits?author=TatoExp" title="Code">💻</a></td>
  </tr>
</table>
