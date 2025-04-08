USE SGCP;
GO

-- ========================================
-- 1. INSERTAR PERMISOS
-- ========================================
INSERT INTO [Sec].[Permissions] ([Name], [InsertDate], [Enable])
SELECT V.Name, GETDATE(), 1
FROM (VALUES
  ('Products.Read'), ('Products.Create'), ('Products.Update'), ('Products.Delete'),
  ('Components.Read'), ('Components.Create'), ('Components.Update'), ('Components.Delete'),
  ('Inventory.Read'), ('Inventory.Create'), ('Inventory.Update'), ('Inventory.Delete'),
  ('Categories.Read'), ('Categories.Create'), ('Categories.Update'), ('Categories.Delete'),
  ('Units.Read'), ('Units.Create'), ('Units.Update'), ('Units.Delete'),
  ('Regions.Read'), ('Regions.Create'), ('Regions.Update'), ('Regions.Delete'),
  ('Users.Read'), ('Users.Create'), ('Users.Update'), ('Users.Delete'),
  ('Roles.Read'), ('Roles.Create'), ('Roles.Update'), ('Roles.Delete'),
  ('Permissions.Read'),
  ('Prices.Read'), ('Prices.Update'),
  ('Security.Manage'), ('Security.Audit')
) AS V(Name)
WHERE NOT EXISTS (
  SELECT 1 FROM [Sec].[Permissions] WHERE [Name] = V.Name
);
GO

-- ========================================
-- 2. CREAR ROLES
-- ========================================
INSERT INTO [Sec].[Roles] ([Name], [Description], [InsertDate], [Enable])
SELECT V.Name, V.Description, GETDATE(), 1
FROM (VALUES
  ('Admin', 'Full access to everything'),
  ('User', 'Read-only access'),
  ('Editor', 'Read and update access')
) AS V(Name, Description)
WHERE NOT EXISTS (
  SELECT 1 FROM [Sec].[Roles] WHERE [Name] = V.Name
);
GO

-- ========================================
-- 3. ASOCIAR PERMISOS A ROLES
-- ========================================

-- Admin: todos los permisos
INSERT INTO [Sec].[RolePermissions] (RoleId, PermissionId)
SELECT R.Id, P.Id
FROM [Sec].[Roles] R
JOIN [Sec].[Permissions] P ON 1 = 1
WHERE R.Name = 'Admin'
AND NOT EXISTS (
    SELECT 1 FROM [Sec].[RolePermissions] RP WHERE RP.RoleId = R.Id AND RP.PermissionId = P.Id
);

-- User: solo .Read
INSERT INTO [Sec].[RolePermissions] (RoleId, PermissionId)
SELECT R.Id, P.Id
FROM [Sec].[Roles] R
JOIN [Sec].[Permissions] P ON P.Name LIKE '%.Read'
WHERE R.Name = 'User'
AND NOT EXISTS (
    SELECT 1 FROM [Sec].[RolePermissions] RP WHERE RP.RoleId = R.Id AND RP.PermissionId = P.Id
);

-- Editor: .Read y .Update
INSERT INTO [Sec].[RolePermissions] (RoleId, PermissionId)
SELECT R.Id, P.Id
FROM [Sec].[Roles] R
JOIN [Sec].[Permissions] P ON P.Name LIKE '%.Read' OR P.Name LIKE '%.Update'
WHERE R.Name = 'Editor'
AND NOT EXISTS (
    SELECT 1 FROM [Sec].[RolePermissions] RP WHERE RP.RoleId = R.Id AND RP.PermissionId = P.Id
);
GO

-- ========================================
-- 4. USUARIO ADMIN
-- ========================================
INSERT INTO [Sec].[Users] (Username, PasswordHash, Email, Enable, InsertDate)
SELECT 'admin', 'admin123', 'admin@example.com', 1, GETDATE()
WHERE NOT EXISTS (
    SELECT 1 FROM [Sec].[Users] WHERE Username = 'admin'
);

-- Asociar rol Admin
INSERT INTO [Sec].[UserRoles] (UserId, RoleId)
SELECT U.Id, R.Id
FROM [Sec].[Users] U, [Sec].[Roles] R
WHERE U.Username = 'admin' AND R.Name = 'Admin'
AND NOT EXISTS (
    SELECT 1 FROM [Sec].[UserRoles] UR WHERE UR.UserId = U.Id AND UR.RoleId = R.Id
);
GO