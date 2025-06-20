# LocalDB Connection String Alternatives

If you continue to have connection issues, try these alternative connection strings:

## Option 1: Current (should work now)
```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ChocoArtesaniasDb;Trusted_Connection=true;MultipleActiveResultSets=true"
```

## Option 2: Using Data Source
```json
"DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ChocoArtesaniasDb;Integrated Security=True;Multiple Active Result Sets=True"
```

## Option 3: Using the pipe name (from sqllocaldb info)
```json
"DefaultConnection": "Server=np:\\\\\\\\pipe\\\\LOCALDB#B7BD6DF1\\\\tsql\\\\query;Database=ChocoArtesaniasDb;Trusted_Connection=true;MultipleActiveResultSets=true"
```

## Option 4: If you want to use SQL Server Express instead
```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=ChocoArtesaniasDb;Trusted_Connection=true;MultipleActiveResultSets=true"
```

## Troubleshooting Commands

1. List LocalDB instances:
   ```cmd
   sqllocaldb info
   ```

2. Check specific instance:
   ```cmd
   sqllocaldb info MSSQLLocalDB
   ```

3. Start LocalDB instance:
   ```cmd
   sqllocaldb start MSSQLLocalDB
   ```

4. Stop LocalDB instance:
   ```cmd
   sqllocaldb stop MSSQLLocalDB
   ```

5. Delete and recreate instance:
   ```cmd
   sqllocaldb delete MSSQLLocalDB
   sqllocaldb create MSSQLLocalDB
   ```
