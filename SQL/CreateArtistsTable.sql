create table Artists(
        Id INT PRIMARY KEY IDENTITY(1,1),
        "Name" NVARCHAR(255) NOT NULL,
        Bio NVARCHAR(255) NOT NULL,
        ProfileImage NVARCHAR(255) NOT NULL
);