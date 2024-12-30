USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetPaginatedProducts]    Script Date: 30-12-2024 08:51:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetPaginatedProducts]
    @Search NVARCHAR(255),
    @Start INT,
    @End INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * 
    FROM (
        SELECT ROW_NUMBER() OVER (ORDER BY ProductId DESC) AS RowNum, * 
        FROM Products 
        WHERE Name LIKE '%' + @Search + '%'
    ) AS Result 
    WHERE RowNum BETWEEN @Start AND @End;
END;
GO


