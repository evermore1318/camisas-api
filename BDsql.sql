-- Crear la base de datos
USE MASTER
GO
IF EXISTS(SELECT * FROM sys.databases WHERE name='bd_palaciocamisas')
	DROP DATABASE bd_palaciocamisas
GO
CREATE DATABASE bd_palaciocamisas
GO
USE bd_palaciocamisas
GO

-- ========================
-- TABLAS PRINCIPALES
-- ========================

CREATE TABLE ESTANTE (
    id_estante INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(10) NOT NULL,
    estado VARCHAR(20) NOT NULL
)
GO

CREATE TABLE MARCA (
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(45) NOT NULL,
    estado VARCHAR(20) NOT NULL
)
GO

CREATE TABLE CAMISA (
    id_camisa INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(45) NOT NULL,
    id_marca INT NOT NULL,
    color VARCHAR(45) NOT NULL,
    talla VARCHAR(10) NOT NULL,
    manga VARCHAR(10) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    precio_costo DECIMAL(7,2) NOT NULL,
    precio_venta DECIMAL(7,2) NOT NULL,
    id_estante INT NOT NULL,
    estado VARCHAR(30) NOT NULL,

    CONSTRAINT fk_camisa_marca FOREIGN KEY (id_marca) REFERENCES MARCA(id_marca),
    CONSTRAINT fk_camisa_estante FOREIGN KEY (id_estante) REFERENCES ESTANTE(id_estante)
)
GO

CREATE TABLE PROVEEDOR (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    id_marca INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    direccion VARCHAR(100),
    email VARCHAR(100),
    estado VARCHAR(20) NOT NULL,
    CONSTRAINT fk_proveedor_marca FOREIGN KEY (id_marca) REFERENCES MARCA(id_marca)
)
GO

CREATE TABLE PEDIDO (
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    id_proveedor INT NOT NULL,
    fecha DATE NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) NOT NULL,
    CONSTRAINT fk_pedido_proveedor FOREIGN KEY (id_proveedor) REFERENCES PROVEEDOR(id_proveedor)
)
GO

CREATE TABLE PAGO (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    fecha DATE NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) NOT NULL,
    CONSTRAINT fk_pago_pedido FOREIGN KEY (id_pedido) REFERENCES PEDIDO(id_pedido)
)
GO

CREATE TABLE VENTA (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    nombre_cliente VARCHAR(100) NOT NULL,
    dni_cliente VARCHAR(15) NOT NULL,
    tipo_pago VARCHAR(40) NOT NULL,
    fecha DATE NOT NULL,
    precio_total DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) NOT NULL
)
GO

CREATE TABLE DETALLEVENTA (
    id_venta INT NOT NULL,
    id_camisa INT NOT NULL,
    cantidad INT NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) NOT NULL,

    PRIMARY KEY (id_venta, id_camisa),
    CONSTRAINT fk_detalleventa_venta FOREIGN KEY (id_venta) REFERENCES VENTA(id_venta),
    CONSTRAINT fk_detalleventa_camisa FOREIGN KEY (id_camisa) REFERENCES CAMISA(id_camisa)
)
GO

CREATE TABLE ROL (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(60) NOT NULL,
    estado VARCHAR(20) NOT NULL
)
GO

CREATE TABLE USUARIO (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    id_rol INT NOT NULL,
    estado VARCHAR(20) NOT NULL,
    
    CONSTRAINT fk_usuario_rol FOREIGN KEY (id_rol) REFERENCES ROL(id_rol)
)
GO

-- ========================
-- INSERCIONES
-- ========================

SET IDENTITY_INSERT MARCA ON
INSERT INTO MARCA (id_marca, descripcion, estado) VALUES
(1, 'Masterly', 'DISPONIBLE'),
(2, 'Requena', 'DISPONIBLE'),
(3, 'Morgan', 'DISPONIBLE'),
(4, 'Nice', 'DISPONIBLE'),
(5, 'John Holden', 'DISPONIBLE'),
(6, 'John Jairo', 'DISPONIBLE'),
(7, 'Cavalier', 'DISPONIBLE'),
(8, 'Alexander', 'DISPONIBLE'),
(9, 'Smith', 'DISPONIBLE'),
(10, 'Galton', 'DISPONIBLE'),
(11, 'Doger', 'DISPONIBLE')
SET IDENTITY_INSERT MARCA OFF
GO

SET IDENTITY_INSERT ESTANTE ON
INSERT INTO ESTANTE (id_estante, descripcion, estado) VALUES
(1, 'A1', 'DISPONIBLE'),
(2, 'A2', 'DISPONIBLE'),
(3, 'A3', 'DISPONIBLE'),
(4, 'A4', 'DISPONIBLE'),
(5, 'B1', 'DISPONIBLE'),
(6, 'B2', 'DISPONIBLE'),
(7, 'B3', 'DISPONIBLE'),
(8, 'B4', 'DISPONIBLE'),
(9, 'C1', 'DISPONIBLE'),
(10, 'C2', 'DISPONIBLE'),
(11, 'C3', 'DISPONIBLE'),
(12, 'C4', 'DISPONIBLE')
SET IDENTITY_INSERT ESTANTE OFF
GO

SET IDENTITY_INSERT CAMISA ON
INSERT INTO CAMISA (id_camisa, descripcion, id_marca, color, talla, manga, stock, precio_costo, precio_venta, id_estante, estado) VALUES
-- MASTERLY
(1, 'Color-Entero', 1, 'Rojo', 'M', 'Corta', 20, 35, 45, 1, 'DISPONIBLE'),
(2, 'Color-Entero', 1, 'Negro', 'L', 'Larga', 25, 40, 50, 2, 'DISPONIBLE'),
(3, 'Rayas', 1, 'Negro/Rojo', 'S', 'Larga', 15, 35, 45, 3, 'DISPONIBLE'),
(4, 'Cuadros', 1, 'Azul/Celeste', 'XL', 'Corta', 18, 40, 50, 4, 'DISPONIBLE'),

-- MORGAN
(5, 'Color-Entero', 3, 'Blanco', 'M', 'Larga', 30, 35, 45, 5, 'DISPONIBLE'),
(6, 'Color-Entero', 3, 'Verde', 'L', 'Corta', 22, 40, 50, 6, 'DISPONIBLE'),
(7, 'Rayas', 3, 'Azul/Celeste', 'S', 'Corta', 14, 40, 50, 7, 'DISPONIBLE'),
(8, 'Cuadros', 3, 'Negro/Blanco', 'XL', 'Larga', 19, 35, 45, 8, 'DISPONIBLE'),

-- REQUENA
(9, 'Color-Entero', 2, 'Rojo', 'S', 'Corta', 12, 25, 35, 9, 'DISPONIBLE'),
(10, 'Rayas', 2, 'Azul/Celeste', 'M', 'Larga', 28, 25, 35, 10, 'DISPONIBLE'),
(11, 'Cuadros', 2, 'Verde/Negro', 'XL', 'Corta', 32, 25, 35, 11, 'DISPONIBLE'),
(12, 'Color-Entero', 2, 'Blanco', 'L', 'Larga', 18, 25, 35, 12, 'DISPONIBLE'),

-- NICE
(13, 'Color-Entero', 4, 'Negro', 'M', 'Larga', 40, 25, 35, 1, 'DISPONIBLE'),
(14, 'Rayas', 4, 'Rojo/Blanco', 'S', 'Corta', 15, 25, 35, 2, 'DISPONIBLE'),
(15, 'Cuadros', 4, 'Azul/Verde', 'XL', 'Larga', 22, 25, 35, 3, 'DISPONIBLE'),
(16, 'Color-Entero', 4, 'Azul', 'L', 'Corta', 19, 25, 35, 4, 'DISPONIBLE'),

-- JOHN HOLDEN
(17, 'Color-Entero', 5, 'Negro', 'XL', 'Larga', 35, 65, 75, 5, 'DISPONIBLE'),
(18, 'Rayas', 5, 'Rojo/Negro', 'M', 'Corta', 16, 65, 75, 6, 'DISPONIBLE'),
(19, 'Cuadros', 5, 'Verde/Blanco', 'L', 'Larga', 28, 65, 75, 7, 'DISPONIBLE'),
(20, 'Color-Entero', 5, 'Blanco', 'S', 'Corta', 12, 65, 75, 8, 'DISPONIBLE'),

-- JOHN JAIRO
(21, 'Color-Entero', 6, 'Rojo', 'M', 'Larga', 22, 65, 75, 9, 'DISPONIBLE'),
(22, 'Rayas', 6, 'Azul/Negro', 'L', 'Corta', 18, 65, 75, 10, 'DISPONIBLE'),
(23, 'Cuadros', 6, 'Blanco/Negro', 'XL', 'Larga', 30, 65, 75, 11, 'DISPONIBLE'),
(24, 'Color-Entero', 6, 'Verde', 'S', 'Corta', 14, 65, 75, 12, 'DISPONIBLE'),

-- CAVALIER
(25, 'Color-Entero', 7, 'Blanco', 'M', 'Corta', 20, 35, 45, 1, 'DISPONIBLE'),
(26, 'Rayas', 7, 'Rojo/Azul', 'L', 'Larga', 24, 40, 50, 2, 'DISPONIBLE'),
(27, 'Cuadros', 7, 'Negro/Verde', 'XL', 'Corta', 16, 40, 50, 3, 'DISPONIBLE'),
(28, 'Color-Entero', 7, 'Negro', 'S', 'Larga', 18, 35, 45, 4, 'DISPONIBLE'),

-- ALEXANDER
(29, 'Color-Entero', 8, 'Rojo', 'L', 'Corta', 27, 40, 50, 5, 'DISPONIBLE'),
(30, 'Rayas', 8, 'Azul/Blanco', 'M', 'Larga', 22, 35, 45, 6, 'DISPONIBLE'),
(31, 'Cuadros', 8, 'Verde/Negro', 'S', 'Corta', 15, 40, 50, 7, 'DISPONIBLE'),
(32, 'Color-Entero', 8, 'Blanco', 'XL', 'Larga', 29, 35, 45, 8, 'DISPONIBLE'),

-- SMITH
(33, 'Color-Entero', 9, 'Negro', 'S', 'Corta', 13, 25, 35, 9, 'DISPONIBLE'),
(34, 'Rayas', 9, 'Azul/Verde', 'M', 'Larga', 19, 25, 35, 10, 'DISPONIBLE'),
(35, 'Cuadros', 9, 'Rojo/Blanco', 'L', 'Corta', 21, 25, 35, 11, 'DISPONIBLE'),
(36, 'Color-Entero', 9, 'Blanco', 'XL', 'Larga', 32, 25, 35, 12, 'DISPONIBLE'),

-- GALTON
(37, 'Color-Entero', 10, 'Rojo', 'M', 'Larga', 23, 35, 45, 1, 'DISPONIBLE'),
(38, 'Rayas', 10, 'Negro/Azul', 'L', 'Corta', 18, 40, 50, 2, 'DISPONIBLE'),
(39, 'Cuadros', 10, 'Verde/Blanco', 'S', 'Larga', 16, 35, 45, 3, 'DISPONIBLE'),
(40, 'Color-Entero', 10, 'Blanco', 'XL', 'Corta', 27, 40, 50, 4, 'DISPONIBLE'),

-- DOGER
(41, 'Color-Entero', 11, 'Negro', 'S', 'Larga', 12, 25, 35, 5, 'DISPONIBLE'),
(42, 'Rayas', 11, 'Rojo/Azul', 'M', 'Corta', 20, 25, 35, 6, 'DISPONIBLE'),
(43, 'Cuadros', 11, 'Blanco/Verde', 'L', 'Larga', 28, 25, 35, 7, 'DISPONIBLE'),
(44, 'Color-Entero', 11, 'Azul', 'XL', 'Corta', 15, 25, 35, 8, 'DISPONIBLE')
SET IDENTITY_INSERT CAMISA OFF
GO

SET IDENTITY_INSERT PROVEEDOR ON
INSERT INTO PROVEEDOR (id_proveedor, id_marca, nombre, telefono, direccion, email, estado)
VALUES
(1, 1, 'Textiles Andinos SAC', '987654321', 'Av. Abancay 120, Lima', 'textiles.andinos@gmail.com', 'DISPONIBLE'),
(2, 2, 'Confecciones Requena SRL', '912345678', 'Av. Arequipa 456, Lima', 'requena.confecciones@gmail.com', 'DISPONIBLE'),
(3, 3, 'Morgan Clothes EIRL', '998877665', 'Av. Universitaria 890, Lima', 'morgan.clothes@gmail.com', 'DISPONIBLE'),
(4, 4, 'Nice Peru SAC', '934567890', 'Av. La Marina 320, Lima', 'niceperu@gmail.com', 'DISPONIBLE'),
(5, 5, 'John Holden Fashion SRL', '976543210', 'Av. Javier Prado 700, Lima', 'johnholdenfashion@gmail.com', 'DISPONIBLE'),
(6, 6, 'John Jairo Textiles SAC', '955443322', 'Av. Colonial 1500, Lima', 'johnjairo.textiles@gmail.com', 'DISPONIBLE'),
(7, 7, 'Cavalier Style SRL', '964332211', 'Av. Brasil 1234, Lima', 'cavalier.style@gmail.com', 'DISPONIBLE'),
(8, 8, 'Alexander Import EIRL', '976112233', 'Av. Angamos 890, Lima', 'alexander.imports@gmail.com', 'DISPONIBLE'),
(9, 9, 'Smith Confecciones SAC', '987221144', 'Av. Túpac Amaru 560, Lima', 'smith.confecciones@gmail.com', 'DISPONIBLE'),
(10, 10, 'Galton Moda SRL', '945667788', 'Av. Benavides 450, Lima', 'galton.moda@gmail.com', 'DISPONIBLE'),
(11, 11, 'Doger Textiles SAC', '956778899', 'Av. Petit Thouars 1300, Lima', 'doger.textiles@gmail.com', 'DISPONIBLE')
SET IDENTITY_INSERT PROVEEDOR OFF
GO

SET IDENTITY_INSERT PEDIDO ON
INSERT INTO PEDIDO (id_pedido, descripcion, id_proveedor, fecha, monto, estado)
VALUES 
(1, 'Pedido-300-Camisas', 1, '2024-11-10', 6000.00, 'Cancelado'),
(2, 'Pedido-40-Camisas', 2, '2025-04-05', 1000.00, 'Deuda pendiente'),
(3, 'Pedido-25-Camisas', 3, '2025-05-12', 875.00, 'Deuda pendiente'),
(4, 'Pedido-50-Camisas', 4, '2025-06-10', 1250.00, 'Deuda pendiente'),
(5, 'Pedido-30-Camisas', 5, '2025-07-05', 1950.00, 'Deuda pendiente')
SET IDENTITY_INSERT PEDIDO OFF
GO

SET IDENTITY_INSERT PAGO ON
INSERT INTO PAGO (id_pago, id_pedido, descripcion, fecha, monto, estado)
VALUES
(1, 1, 'Primer pago', '2025-01-15', 3000.00, 'Activo'),
(2, 1, 'Segundo pago', '2025-02-15', 1500.00, 'Activo'),
(3, 1, 'Tercer pago', '2025-03-15', 1500.00, 'Activo'),
(4, 2, 'Primer pago', '2025-04-20', 500.00, 'Activo'),
(5, 3, 'Primer pago', '2025-05-25', 400.00, 'Activo'),
(6, 3, 'Segundo pago', '2025-06-25', 200.00, 'Activo'),
(7, 4, 'Primer pago', '2025-06-20', 700.00, 'Activo'),
(8, 5, 'Primer pago', '2025-07-18', 1000.00, 'Activo'),
(9, 5, 'Segundo pago', '2025-08-10', 500.00, 'Activo')
SET IDENTITY_INSERT PAGO OFF
GO

SET IDENTITY_INSERT VENTA ON
INSERT INTO VENTA (id_venta, nombre_cliente, dni_cliente, tipo_pago, fecha, precio_total, estado)
VALUES 
(1, 'Javier Solis', '46892275', 'Yape', '2025-08-20', 90.00, 'Activo'),
(2, 'Luis Andrade', '42158796', 'Efectivo', '2025-08-21', 135.00, 'Activo'),
(3, 'Rosa Gamarra', '50211874', 'Plin', '2025-08-22', 300.00, 'Activo'),
(4, 'Carla Ramos', '47891236', 'Tarjeta', '2025-08-23', 120.00, 'Activo'),
(5, 'Pedro Castillo', '49382716', 'Efectivo', '2025-08-24', 120.00, 'Activo'),
(6, 'María López', '46781239', 'Yape', '2025-08-25', 150.00, 'Activo')
SET IDENTITY_INSERT VENTA OFF
GO

INSERT INTO DETALLEVENTA (id_venta, id_camisa, cantidad, precio, estado)
VALUES 
(1, 1, 2, 90.00, 'Activo'),
(2, 5, 3, 135.00, 'Activo'),
(3, 17, 4, 300.00, 'Activo'),
(4, 2, 1, 50.00, 'Activo'),
(4, 13, 2, 70.00, 'Activo'),
(5, 9, 2, 70.00, 'Activo'),
(5, 7, 1, 50.00, 'Activo'),
(6, 33, 1, 35.00, 'Activo'),
(6, 37, 1, 45.00, 'Activo'),
(6, 41, 2, 70.00, 'Activo')
GO

SET IDENTITY_INSERT ROL ON
INSERT INTO ROL (id_rol, descripcion, estado) VALUES
(1, 'Admin', 'Activo'),
(2, 'Recepcion', 'Activo'),
(3, 'Deposito', 'Activo'),
(4, 'Cajero', 'Activo')
SET IDENTITY_INSERT ROL OFF
GO

SET IDENTITY_INSERT USUARIO ON
INSERT INTO USUARIO (id_usuario, nombre, password, id_rol, estado) VALUES
(1, 'Anthony Sanchez', 'antsan123', 1, 'Activo'),
(2, 'Manfredo Sanchez', 'mansan456', 1, 'Activo'),
(3, 'Angie Sanchez', 'angsan789', 1, 'Activo'),
(4, 'Juana Valcarcel', 'juaval321', 1, 'Activo')
SET IDENTITY_INSERT USUARIO OFF
GO