USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[sp_InsertProduct]    Script Date: 30-12-2024 08:49:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_InsertProduct]
    @Name NVARCHAR(100),
    @Amount DECIMAL(18, 2),
    @Description NVARCHAR(MAX),
    @ImagePath NVARCHAR(200)
AS
BEGIN
    INSERT INTO Products (Name, Amount, Description, ImagePath)
    VALUES (@Name, @Amount, @Description, @ImagePath);
END;
GO


