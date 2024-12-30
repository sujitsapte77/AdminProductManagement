USE [AdminProduct]
GO

/****** Object:  StoredProcedure [dbo].[sp_DeleteProduct]    Script Date: 30-12-2024 08:47:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_DeleteProduct]
    @ProductId INT
AS
BEGIN
    DELETE FROM Products
    WHERE ProductId = @ProductId;
END;



GO


