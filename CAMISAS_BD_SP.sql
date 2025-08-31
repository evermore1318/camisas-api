USE bd_palaciocamisas
GO

CREATE OR ALTER PROCEDURE  ListarVentasConDetalles
AS
BEGIN
    SELECT 
        v.id_venta,
        v.nombre_cliente,
        v.dni_cliente,
        v.tipo_pago,
        v.fecha,
        v.precio_total,
        v.estado,
        dv.id_camisa,
        dv.cantidad,
        dv.precio as precio_detalle,
        dv.estado as estado_detalle,
        c.descripcion as camisa_descripcion,
        c.color as camisa_color,
        c.talla as camisa_talla,
        c.manga as camisa_manga,
        m.descripcion as marca_nombre
    FROM VENTA v
    LEFT JOIN DETALLEVENTA dv ON v.id_venta = dv.id_venta
    LEFT JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    LEFT JOIN MARCA m ON c.id_marca = m.id_marca
    ORDER BY v.fecha DESC, v.id_venta, dv.id_camisa
END
go

CREATE OR ALTER PROCEDURE ObtenerVentaPorIDConDetalles
    @id_venta INT
AS
BEGIN
    SELECT 
		v.id_venta,          
        v.nombre_cliente,     
        v.dni_cliente,       
        v.tipo_pago,         
        v.fecha,              
        v.precio_total,       
        v.estado,             
        dv.id_camisa,         
        dv.cantidad,          
        dv.precio,            
        dv.estado,            
        c.descripcion,        
        c.color,              
        c.talla,              
        c.manga,              
        m.descripcion         
    FROM VENTA v
    LEFT JOIN DETALLEVENTA dv ON v.id_venta = dv.id_venta
    LEFT JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    LEFT JOIN MARCA m ON c.id_marca = m.id_marca
    WHERE v.id_venta = @id_venta
    ORDER BY dv.id_camisa
END
go

CREATE OR ALTER PROCEDURE ObtenerDetallesPorVenta
    @id_venta INT
AS
BEGIN
    SELECT 
        dv.id_camisa,        
        dv.cantidad,        
        dv.precio,           
        dv.estado,            
        c.descripcion,        
        c.color,              
        c.talla,              
        c.manga,              
        m.descripcion         
    FROM DETALLEVENTA dv
    INNER JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    INNER JOIN MARCA m ON c.id_marca = m.id_marca
    WHERE dv.id_venta = @id_venta
END
go

CREATE OR ALTER PROCEDURE ActualizarEstadoVenta
    @id_venta INT,
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE VENTA 
    SET estado = @estado
    WHERE id_venta = @id_venta
END
GO

CREATE OR ALTER PROCEDURE ActualizarEstadoDetalleVenta
    @id_venta INT,
    @estado VARCHAR(20)
AS
BEGIN
    UPDATE DETALLEVENTA 
    SET estado = @estado
    WHERE id_venta = @id_venta
END
GO

-- SP para Marcas
CREATE OR ALTER PROCEDURE ListarMarcas
AS
BEGIN
    SELECT id_marca, descripcion, estado
    FROM MARCA
    WHERE estado = 'DISPONIBLE'
    ORDER BY descripcion
END
go

-- SP para Estantes
CREATE OR ALTER PROCEDURE ListarEstantes
AS
BEGIN
    SELECT id_estante, descripcion, estado
    FROM ESTANTE
    WHERE estado = 'DISPONIBLE'
    ORDER BY descripcion
END
GO

-- SP para Camisas
CREATE OR ALTER PROCEDURE ListarCamisas
AS
BEGIN
    SELECT 
        c.id_camisa,
        c.descripcion,
        c.id_marca,
        c.color,
        c.talla,
        c.manga,
        c.stock,
        c.precio_costo,
        c.precio_venta,
        c.id_estante,
        c.estado,
        m.descripcion as marca_nombre,
        e.descripcion as estante_descripcion
    FROM CAMISA c
    INNER JOIN MARCA m ON c.id_marca = m.id_marca
    INNER JOIN ESTANTE e ON c.id_estante = e.id_estante
    WHERE c.estado = 'DISPONIBLE'
    ORDER BY m.descripcion, c.descripcion
END
GO

CREATE OR ALTER PROCEDURE ObtenerCamisaPorID
    @id_camisa INT
AS
BEGIN
    SELECT 
        c.id_camisa,
        c.descripcion,
        c.id_marca,
        c.color,
        c.talla,
        c.manga,
        c.stock,
        c.precio_costo,
        c.precio_venta,
        c.id_estante,
        c.estado,
        m.descripcion as marca_nombre,
        e.descripcion as estante_descripcion
    FROM CAMISA c
    INNER JOIN MARCA m ON c.id_marca = m.id_marca
    INNER JOIN ESTANTE e ON c.id_estante = e.id_estante
    WHERE c.id_camisa = @id_camisa
END
GO

CREATE OR ALTER PROCEDURE RegistrarCamisa
    @descripcion VARCHAR(45),
    @id_marca INT,
    @color VARCHAR(45),
    @talla VARCHAR(10),
    @manga VARCHAR(10),
    @stock INT,
    @precio_costo DECIMAL(7,2),
    @precio_venta DECIMAL(7,2),
    @id_estante INT,
    @estado VARCHAR(30)
AS
BEGIN
    INSERT INTO CAMISA (descripcion, id_marca, color, talla, manga, stock, precio_costo, precio_venta, id_estante, estado)
    VALUES (@descripcion, @id_marca, @color, @talla, @manga, @stock, @precio_costo, @precio_venta, @id_estante, @estado)
    
    SELECT SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE ActualizarCamisa
    @id_camisa INT,
    @descripcion VARCHAR(45),
    @id_marca INT,
    @color VARCHAR(45),
    @talla VARCHAR(10),
    @manga VARCHAR(10),
    @stock INT,
    @precio_costo DECIMAL(7,2),
    @precio_venta DECIMAL(7,2),
    @id_estante INT,
    @estado VARCHAR(30)
AS
BEGIN
    UPDATE CAMISA 
    SET descripcion = @descripcion,
        id_marca = @id_marca,
        color = @color,
        talla = @talla,
        manga = @manga,
        stock = @stock,
        precio_costo = @precio_costo,
        precio_venta = @precio_venta,
        id_estante = @id_estante,
        estado = @estado
    WHERE id_camisa = @id_camisa
END
GO

CREATE OR ALTER PROCEDURE EliminarCamisa
    @id_camisa INT
AS
BEGIN
    UPDATE CAMISA 
    SET estado = 'ELIMINADO'
    WHERE id_camisa = @id_camisa
END
GO

-- SP para Ventas
CREATE OR ALTER PROCEDURE ListarVentas
AS
BEGIN
    SELECT 
        id_venta,
        nombre_cliente,
        dni_cliente,
        tipo_pago,
        fecha,
        precio_total,
        estado
    FROM VENTA
    WHERE estado = 'Activo'
    ORDER BY fecha DESC
END
GO

CREATE OR ALTER PROCEDURE ObtenerVentaPorID
    @id_venta INT
AS
BEGIN
    SELECT 
        v.id_venta,
        v.nombre_cliente,
        v.dni_cliente,
        v.tipo_pago,
        v.fecha,
        v.precio_total,
        v.estado
    FROM VENTA v
    WHERE v.id_venta = @id_venta
    
    -- Tambi n obtener los detalles
    SELECT 
        dv.id_venta,
        dv.id_camisa,
        dv.cantidad,
        dv.precio,
        dv.estado,
        c.descripcion as camisa_descripcion,
        c.color as camisa_color,
        c.talla as camisa_talla
    FROM DETALLEVENTA dv
    INNER JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    WHERE dv.id_venta = @id_venta
END
GO

CREATE OR ALTER PROCEDURE RegistrarVenta
    @nombre_cliente VARCHAR(100),
    @dni_cliente VARCHAR(15),
    @tipo_pago VARCHAR(40),
    @fecha DATE,
    @precio_total DECIMAL(10,2),
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO VENTA (nombre_cliente, dni_cliente, tipo_pago, fecha, precio_total, estado)
    VALUES (@nombre_cliente, @dni_cliente, @tipo_pago, @fecha, @precio_total, @estado)
    
    SELECT SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE RegistrarDetalleVenta
    @id_venta INT,
    @id_camisa INT,
    @cantidad INT,
    @precio DECIMAL(10,2),
    @estado VARCHAR(20)
AS
BEGIN
    INSERT INTO DETALLEVENTA (id_venta, id_camisa, cantidad, precio, estado)
    VALUES (@id_venta, @id_camisa, @cantidad, @precio, @estado)
    
    -- Actualizar stock de la camisa
    UPDATE CAMISA 
    SET stock = stock - @cantidad
    WHERE id_camisa = @id_camisa
END
GO


----------------------------------------------------
-- PROCEDIMIENTOS ALMACENADOS 2 MIGUEL Y NAYELI

ALTER PROCEDURE LoginUsuario
    @Usuario NVARCHAR(50),
    @Clave NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 
        U.id_usuario,
        U.nombre,
        R.descripcion
    FROM USUARIO U
    INNER JOIN ROL R ON U.id_rol = R.id_rol
    WHERE U.nombre = @Usuario
      AND U.password = @Clave
      AND LOWER(U.estado) = 'activo';
END
GO

CREATE OR ALTER PROC ListarRoles
AS
BEGIN
    SELECT id_rol AS IdRol,
           descripcion AS Descripcion,
           estado AS Estado
    FROM ROL
    ORDER BY descripcion;
END
GO


CREATE OR ALTER PROC ListarUsuarios
AS
BEGIN
    SELECT  u.id_usuario AS IdUsuario,
            u.nombre     AS Nombre,
            u.id_rol     AS IdRol,
            r.descripcion AS RolDescripcion,
            u.estado     AS Estado
    FROM USUARIO u
    INNER JOIN ROL r ON u.id_rol = r.id_rol;
END
GO


CREATE OR ALTER PROC ObtenerUsuarioPorID
    @Id INT
AS
BEGIN
    SELECT  u.id_usuario AS IdUsuario,
            u.nombre     AS Nombre,
            u.id_rol     AS IdRol,
            r.descripcion AS RolDescripcion,
            u.estado     AS Estado
    FROM USUARIO u
    INNER JOIN ROL r ON u.id_rol = r.id_rol
    WHERE u.id_usuario = @Id;
END
GO


CREATE OR ALTER PROC RegistrarUsuario
    @Nombre    VARCHAR(100),
    @Password  VARCHAR(100),
    @IdRol     INT,
    @Estado    VARCHAR(20)
AS
BEGIN
    INSERT INTO USUARIO (nombre, password, id_rol, estado)
    VALUES (@Nombre, @Password, @IdRol, @Estado);

    SELECT @@IDENTITY; 
END
GO


CREATE OR ALTER PROC ActualizarEstadoUsuario
    @Id     INT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE USUARIO
       SET estado = @Estado
     WHERE id_usuario = @Id;
END
GO

CREATE OR ALTER PROC EliminarUsuario
    @Id INT
AS
BEGIN
    DELETE FROM USUARIO
    WHERE id_usuario = @Id;
END
GO


CREATE OR ALTER PROC ActualizarPedido
(
    @id INT,
    @descripcion VARCHAR(100),
    @proveedor INT,
    @fecha DATE,
    @monto DECIMAL(10,2),
    @estado VARCHAR(20)
)
AS
BEGIN
    UPDATE Pedido
    SET descripcion = @descripcion,
        id_proveedor = @proveedor,
        fecha = @fecha,
        monto = @monto,
        estado = @estado
    WHERE id_pedido = @id;
END
GO

CREATE OR ALTER PROCEDURE ListarMarcas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id_marca, descripcion, estado
    FROM MARCA
    WHERE estado = 'DISPONIBLE';
END;
GO

CREATE OR ALTER PROCEDURE ListarPedidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado,
        pr.nombre AS nombre_proveedor
    FROM PEDIDO p
    INNER JOIN PROVEEDOR pr ON p.id_proveedor = pr.id_proveedor;
END;
GO

CREATE OR ALTER PROCEDURE ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id_proveedor, id_marca, nombre, telefono, direccion, email, estado
    FROM PROVEEDOR;
END;
GO

CREATE OR ALTER PROCEDURE ObtenerPagoPorID
(
    @ID INT
)
AS
BEGIN
    SELECT 
        id_pago,
        id_pedido,
        descripcion,
        fecha,
        monto,
        estado
    FROM PAGO
    WHERE id_pago = @ID;
END;
GO

CREATE OR ALTER PROCEDURE ObtenerPedidoPorID
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado,
        ISNULL(COUNT(pg.id_pago), 0) AS CantidadPagos,
        ISNULL(SUM(pg.monto), 0) AS MontoTotalPagos,
        p.monto - ISNULL(SUM(pg.monto), 0) AS DeudaPendiente
    FROM Pedido p
    LEFT JOIN Pago pg ON p.id_pedido = pg.id_pedido
    WHERE p.id_pedido = @ID
    GROUP BY 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado;
END;
GO

CREATE OR ALTER PROC RegistrarPago
(
    @idPedido INT,
    @descripcion VARCHAR(100),
    @fecha DATE,
    @monto DECIMAL(10,2),
    @estado VARCHAR(20) = 'Activo'
)
AS
INSERT INTO Pago(id_pedido, descripcion, fecha, monto, estado)
VALUES(@idPedido, @descripcion, @fecha, @monto, @estado);

SELECT SCOPE_IDENTITY();
GO

CREATE OR ALTER PROC RegistrarPedido
(
    @descripcion VARCHAR(250),
    @proveedor INT,
    @fecha DATETIME,
    @monto DECIMAL(10,2),
    @estado VARCHAR(50) = 'Activo'   -- valor por defecto
)
AS
BEGIN
    INSERT INTO Pedido (descripcion, id_proveedor, fecha, monto, estado)
    VALUES (@descripcion, @proveedor, @fecha, @monto, @estado);

    SELECT SCOPE_IDENTITY() AS NuevoId;
END
GO

CREATE OR ALTER PROCEDURE sp_EgresosAnuales
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(monto), 0) AS EgresosAnuales
    FROM PAGO
    WHERE YEAR(fecha) = @Anio
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_EgresosMensuales
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(monto), 0) AS EgresosMensuales
    FROM PAGO
    WHERE YEAR(fecha) = @Anio
      AND MONTH(fecha) = @Mes
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_IngresosMensuales
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(precio_total), 0) AS IngresosMensuales
    FROM VENTA
    WHERE YEAR(fecha) = @Anio
      AND MONTH(fecha) = @Mes
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_IngresosAnuales
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(precio_total), 0) AS IngresosAnuales
    FROM VENTA
    WHERE YEAR(fecha) = @Anio
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_reporte_diario
    @fecha_reporte DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(ROW_NUMBER() OVER (ORDER BY v.id_venta) AS int) AS numero,
        m.descripcion AS marca,
        dv.cantidad,
        dv.precio
    FROM VENTA v
    INNER JOIN DETALLEVENTA dv ON v.id_venta = dv.id_venta
    INNER JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    INNER JOIN MARCA m ON c.id_marca = m.id_marca
    WHERE v.fecha = @fecha_reporte
      AND v.estado = 'Activo'
      AND dv.estado = 'Activo'
    ORDER BY v.id_venta, m.descripcion;
END;
GO


----------------------------------------------------
-- PROCEDIMIENTOS ALMACENADOS 2 MIGUEL Y NAYELI

CREATE OR ALTER PROCEDURE LoginUsuario
    @Usuario NVARCHAR(50),
    @Clave NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 
        U.id_usuario,
        U.nombre,
        R.descripcion
    FROM USUARIO U
    INNER JOIN ROL R ON U.id_rol = R.id_rol
    WHERE U.nombre = @Usuario
      AND U.password = @Clave
      AND LOWER(U.estado) = 'activo';
END
GO

CREATE OR ALTER PROC ListarRoles
AS
BEGIN
    SELECT id_rol AS IdRol,
           descripcion AS Descripcion,
           estado AS Estado
    FROM ROL
    ORDER BY descripcion;
END
GO


CREATE OR ALTER PROC ListarUsuarios
AS
BEGIN
    SELECT  u.id_usuario AS IdUsuario,
            u.nombre     AS Nombre,
            u.id_rol     AS IdRol,
            r.descripcion AS RolDescripcion,
            u.estado     AS Estado
    FROM USUARIO u
    INNER JOIN ROL r ON u.id_rol = r.id_rol;
END
GO


CREATE OR ALTER PROC ObtenerUsuarioPorID
    @Id INT
AS
BEGIN
    SELECT  u.id_usuario AS IdUsuario,
            u.nombre     AS Nombre,
            u.id_rol     AS IdRol,
            r.descripcion AS RolDescripcion,
            u.estado     AS Estado
    FROM USUARIO u
    INNER JOIN ROL r ON u.id_rol = r.id_rol
    WHERE u.id_usuario = @Id;
END
GO


CREATE OR ALTER PROC RegistrarUsuario
    @Nombre    VARCHAR(100),
    @Password  VARCHAR(100),
    @IdRol     INT,
    @Estado    VARCHAR(20)
AS
BEGIN
    INSERT INTO USUARIO (nombre, password, id_rol, estado)
    VALUES (@Nombre, @Password, @IdRol, @Estado);

    SELECT @@IDENTITY; 
END
GO


CREATE OR ALTER PROC ActualizarEstadoUsuario
    @Id     INT,
    @Estado VARCHAR(20)
AS
BEGIN
    UPDATE USUARIO
       SET estado = @Estado
     WHERE id_usuario = @Id;
END
GO

CREATE OR ALTER PROC EliminarUsuario
    @Id INT
AS
BEGIN
    DELETE FROM USUARIO
    WHERE id_usuario = @Id;
END
GO


CREATE OR ALTER PROC ActualizarPedido
(
    @id INT,
    @descripcion VARCHAR(100),
    @proveedor INT,
    @fecha DATE,
    @monto DECIMAL(10,2),
    @estado VARCHAR(20)
)
AS
BEGIN
    UPDATE Pedido
    SET descripcion = @descripcion,
        id_proveedor = @proveedor,
        fecha = @fecha,
        monto = @monto,
        estado = @estado
    WHERE id_pedido = @id;
END
GO

CREATE OR ALTER PROCEDURE ListarMarcas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id_marca, descripcion, estado
    FROM MARCA
    WHERE estado = 'DISPONIBLE';
END;
GO

CREATE OR ALTER PROCEDURE ListarPedidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado,
        pr.nombre AS nombre_proveedor
    FROM PEDIDO p
    INNER JOIN PROVEEDOR pr ON p.id_proveedor = pr.id_proveedor;
END;
GO

CREATE OR ALTER PROCEDURE ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id_proveedor, id_marca, nombre, telefono, direccion, email, estado
    FROM PROVEEDOR;
END;
GO

CREATE OR ALTER PROCEDURE ObtenerPagoPorID
(
    @ID INT
)
AS
BEGIN
    SELECT 
        id_pago,
        id_pedido,
        descripcion,
        fecha,
        monto,
        estado
    FROM PAGO
    WHERE id_pago = @ID;
END;
GO

CREATE OR ALTER PROCEDURE ObtenerPedidoPorID
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado,
        ISNULL(COUNT(pg.id_pago), 0) AS CantidadPagos,
        ISNULL(SUM(pg.monto), 0) AS MontoTotalPagos,
        p.monto - ISNULL(SUM(pg.monto), 0) AS DeudaPendiente
    FROM Pedido p
    LEFT JOIN Pago pg ON p.id_pedido = pg.id_pedido
    WHERE p.id_pedido = @ID
    GROUP BY 
        p.id_pedido,
        p.descripcion,
        p.id_proveedor,
        p.fecha,
        p.monto,
        p.estado;
END;
GO

CREATE OR ALTER PROC RegistrarPago
(
    @idPedido INT,
    @descripcion VARCHAR(100),
    @fecha DATE,
    @monto DECIMAL(10,2),
    @estado VARCHAR(20) = 'Activo'
)
AS
INSERT INTO Pago(id_pedido, descripcion, fecha, monto, estado)
VALUES(@idPedido, @descripcion, @fecha, @monto, @estado);

SELECT SCOPE_IDENTITY();
GO

CREATE OR ALTER PROC RegistrarPedido
(
    @descripcion VARCHAR(250),
    @proveedor INT,
    @fecha DATETIME,
    @monto DECIMAL(10,2),
    @estado VARCHAR(50) = 'Activo'   -- valor por defecto
)
AS
BEGIN
    INSERT INTO Pedido (descripcion, id_proveedor, fecha, monto, estado)
    VALUES (@descripcion, @proveedor, @fecha, @monto, @estado);

    SELECT SCOPE_IDENTITY() AS NuevoId;
END
GO

CREATE OR ALTER PROCEDURE sp_EgresosAnuales
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(monto), 0) AS EgresosAnuales
    FROM PAGO
    WHERE YEAR(fecha) = @Anio
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_EgresosMensuales
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(monto), 0) AS EgresosMensuales
    FROM PAGO
    WHERE YEAR(fecha) = @Anio
      AND MONTH(fecha) = @Mes
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_IngresosMensuales
    @Anio INT,
    @Mes INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(precio_total), 0) AS IngresosMensuales
    FROM VENTA
    WHERE YEAR(fecha) = @Anio
      AND MONTH(fecha) = @Mes
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_IngresosAnuales
    @Anio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(precio_total), 0) AS IngresosAnuales
    FROM VENTA
    WHERE YEAR(fecha) = @Anio
      AND estado = 'Activo';
END;
GO

CREATE OR ALTER PROCEDURE sp_reporte_diario
    @fecha_reporte DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(ROW_NUMBER() OVER (ORDER BY v.id_venta) AS int) AS numero,
        m.descripcion AS marca,
        dv.cantidad,
        dv.precio
    FROM VENTA v
    INNER JOIN DETALLEVENTA dv ON v.id_venta = dv.id_venta
    INNER JOIN CAMISA c ON dv.id_camisa = c.id_camisa
    INNER JOIN MARCA m ON c.id_marca = m.id_marca
    WHERE v.fecha = @fecha_reporte
      AND v.estado = 'Activo'
      AND dv.estado = 'Activo'
    ORDER BY v.id_venta, m.descripcion;
END;
GO