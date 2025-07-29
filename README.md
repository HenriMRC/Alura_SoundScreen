# **Course**: Sound Screen Project

Project of course from **Alura**

Course: [.NET: data persistance with Entity Framework Core](https://cursos.alura.com.br/course/dot-net-persistindo-dados-entity-framework-core)

Using `Microsoft.EntityFrameworkCore.* Version="7.0.20"` and `Microsoft.Data.SqlClient Version="5.2.3"` for combility with `net6`

Notes:
- To use a migration go to `Tools > NuGet Package Manager > Package Manager Console`
- Command `Add-Migration <name>` captures model changes since the last migration and generating migration files from those changes. [*Source*](https://www.learnentityframeworkcore.com/migrations/add-migration)
    - Option: `-Project <migration-assembly>`
        - Currently it is the project `screensound.core.data`
- Command `Update-Database <migration-name[optional]>` excutes the 
migration.
    - Option: `-Project <migration-assembly>`
        - Currently it is the project `screensound.core.data`