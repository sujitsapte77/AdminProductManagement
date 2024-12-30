USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetProductById]    Script Date: 30-12-2024 08:48:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_GetProductById]
    @ProductId INT
AS
BEGIN
    SELECT *
    FROM Products
    WHERE ProductId = @ProductId;
END;

GO


