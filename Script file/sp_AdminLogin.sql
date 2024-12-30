USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminLogin]    Script Date: 30-12-2024 08:46:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_AdminLogin]
    @Username NVARCHAR(50),
    @Password NVARCHAR(50) -- Password should be hashed
AS
BEGIN
    SELECT COUNT(1)
    FROM Admins
    WHERE Username = @Username AND Password = @Password;
END;
GO


