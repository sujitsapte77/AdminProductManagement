USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[usp_CountProducts]    Script Date: 30-12-2024 08:50:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_CountProducts]
    @Search NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) 
    FROM Products 
    WHERE Name LIKE '%' + @Search + '%';
END;
GO


