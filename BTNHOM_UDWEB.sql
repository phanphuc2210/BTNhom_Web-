CREATE DATABASE BTNHOM_UDWEB
GO

USE BTNHOM_UDWEB
GO

CREATE TABLE KHACHHANG (
	MAKH VARCHAR(10) PRIMARY KEY,
	TEN NVARCHAR(10) NOT NULL,
	TENDEM NVARCHAR(10) ,
	HO NVARCHAR(10) NOT NULL,
	DIACHI NVARCHAR(100) NOT NULL,
	SDT VARCHAR(12) UNIQUE NOT NULL,
	AVATAR VARCHAR(255),
	EMAIL NVARCHAR(255) UNIQUE,
	PASS VARCHAR(100) 
)


CREATE TABLE NHANVIEN (
	MANV VARCHAR(10) PRIMARY KEY,
	TEN NVARCHAR(10) NOT NULL,
	TENDEM NVARCHAR(10) ,
	HO NVARCHAR(10) NOT NULL,
	NGAYSINH DATE,
	DIACHI NVARCHAR(100) NOT NULL,
	SDT VARCHAR(12) UNIQUE NOT NULL,
	AVATAR VARCHAR(255),
	EMAIL NVARCHAR(255) UNIQUE,
	TENDN VARCHAR(100) UNIQUE NOT NULL,
	PASS VARCHAR(100) NOT NULL,
	IsAdmin BIT NOT NULL
)

CREATE TABLE LOAISP (
	MALSP VARCHAR(10) PRIMARY KEY ,
	TENLSP NVARCHAR(50) NOT NULL
)

CREATE TABLE SANPHAM (
	MASP VARCHAR(10) PRIMARY KEY,
	MALSP VARCHAR(10) NOT NULL FOREIGN KEY (MALSP) REFERENCES LOAISP(MALSP),
	TENSP NVARCHAR(100) NOT NULL,
	SOLUONGTON INT NOT NULL,
	IMG VARCHAR(255),
	MOTA NVARCHAR(500),
	GIABAN INT,
	NGAYTHEM DATE
)

CREATE TABLE BINHLUAN(
	IDBL INT PRIMARY KEY IDENTITY(1,1),
	MAKH VARCHAR(10) NOT NULL FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
	MASP VARCHAR(10) NOT NULL FOREIGN KEY (MASP) REFERENCES SANPHAM(MASP),
	NOIDUNGBL NVARCHAR(255) NOT NULL,
	THOIGIANBL DATETIME
)

CREATE TABLE TINHTRANG (
	MATINHTRANG VARCHAR(10) PRIMARY KEY,
	TENTINHTRANG NVARCHAR(50) NOT NULL
)

CREATE TABLE PHUONGTHUCTT (
	MAPT VARCHAR(10) PRIMARY KEY,
	TENPT NVARCHAR(50) NOT NULL
)

CREATE TABLE HOADON (
	IDHD INT PRIMARY KEY IDENTITY(1,1),
	MAKH VARCHAR(10) NOT NULL FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
	NGAYDATHANG DATETIME,
	NGAYGIAOHANG DATE,
	NOIGIAOHANG NVARCHAR(100) NOT NULL,
	MANVDUYET VARCHAR(10) FOREIGN KEY(MANVDUYET) REFERENCES NHANVIEN(MANV),
	MANVGIAO VARCHAR(10) FOREIGN KEY(MANVGIAO) REFERENCES NHANVIEN(MANV),
	MAPT VARCHAR(10) FOREIGN KEY(MAPT) REFERENCES PHUONGTHUCTT(MAPT),
	TINHTRANG VARCHAR(10) FOREIGN KEY(TINHTRANG) REFERENCES TINHTRANG(MATINHTRANG) NOT NULL
)

CREATE TABLE CTHD (
	IDHD INT NOT NULL FOREIGN KEY (IDHD) REFERENCES HOADON(IDHD),
	MASP VARCHAR(10) NOT NULL FOREIGN KEY (MASP) REFERENCES SANPHAM(MASP),
	SOLUONG INT NOT NULL,
	DONGIA INT NOT NULL,
	PRIMARY KEY(IDHD,MASP)
)


INSERT INTO NHANVIEN(MANV,TEN,TENDEM,HO,NGAYSINH,DIACHI,SDT,AVATAR,EMAIL,TENDN,PASS,IsAdmin) VALUES
('NV001',N'Phúc',N'Trần Hữu',N'Phan','10/22/2001',N'56KA Cù Lao Thượng, Nha Trang','0708488401','employee.png','phucpth2001@gmail.com','nv001','22102001',1),
('NV002',N'Tùng',N'Thanh',N'Nguyễn','7/5/1994',N'53KB Cù Lao Trung, Nha Trang','0120737421','employee.png','tungnt1994@gmail.com','nv002','05071994',0)

INSERT INTO LOAISP VALUES
('BA',N'Bàn'),
('GH',N'Ghế'),
('TU',N'Tủ'),
('GI',N'Giường'),
('KE',N'Kệ')

INSERT INTO SANPHAM(MASP,MALSP,TENSP,SOLUONGTON,IMG,MOTA,GIABAN, NGAYTHEM) VALUES
('SP001','BA',N'Bàn làm viêc A',20,'https://cf.shopee.vn/file/d64043bc8380fbaaaaabd0bdfd8f6dae',N'Bàn là một loại nội thất, với cấu tạo của nó hàm chứa một mặt phẳng nằm ngang (gọi là mặt bàn) có tác dụng dùng để nâng đỡ cho những vật dụng hay vật thể mà người dùng muốn đặt lên mặt bàn đó.',600000, GETDATE()),
('SP002','BA',N'Bàn làm viêc B',20,'https://cf.shopee.vn/file/d64043bc8380fbaaaaabd0bdfd8f6dae',N'Bàn là một loại nội thất, với cấu tạo của nó hàm chứa một mặt phẳng nằm ngang (gọi là mặt bàn) có tác dụng dùng để nâng đỡ cho những vật dụng hay vật thể mà người dùng muốn đặt lên mặt bàn đó.',600000, GETDATE()),
('SP003','BA',N'Bàn làm viêc C',20,'https://cf.shopee.vn/file/d64043bc8380fbaaaaabd0bdfd8f6dae',N'Bàn là một loại nội thất, với cấu tạo của nó hàm chứa một mặt phẳng nằm ngang (gọi là mặt bàn) có tác dụng dùng để nâng đỡ cho những vật dụng hay vật thể mà người dùng muốn đặt lên mặt bàn đó.',600000, GETDATE()),
('SP004','BA',N'Bàn ăn A',20,'https://cf.shopee.vn/file/d64043bc8380fbaaaaabd0bdfd8f6dae',N'Bàn là một loại nội thất, với cấu tạo của nó hàm chứa một mặt phẳng nằm ngang (gọi là mặt bàn) có tác dụng dùng để nâng đỡ cho những vật dụng hay vật thể mà người dùng muốn đặt lên mặt bàn đó.',500000, GETDATE()),
('SP005','GH',N'Ghế làm việc A',20,'https://cf.shopee.vn/file/d3e13bebaa20adc46dc1c22cd4ec069c',N'Thông thường ghế có bốn chân. Ngoài ra có ghế ba chân và cũng có thể có ghế nhiều chân hơn nữa, nhưng hiếm. Có các loại ghế "một chân" hay "hai chân" nếu "chân ghế" có hình dạng đủ để tạo thành chân đế bền vững chống đỡ cho cấu trúc không bị đổ.',400000, GETDATE()),
('SP006','GH',N'Ghế làm việc B',20,'https://cf.shopee.vn/file/d3e13bebaa20adc46dc1c22cd4ec069c',N'Thông thường ghế có bốn chân. Ngoài ra có ghế ba chân và cũng có thể có ghế nhiều chân hơn nữa, nhưng hiếm. Có các loại ghế "một chân" hay "hai chân" nếu "chân ghế" có hình dạng đủ để tạo thành chân đế bền vững chống đỡ cho cấu trúc không bị đổ.',400000, GETDATE()),
('SP007','GH',N'Ghế làm việc C',20,'https://cf.shopee.vn/file/d3e13bebaa20adc46dc1c22cd4ec069c',N'Thông thường ghế có bốn chân. Ngoài ra có ghế ba chân và cũng có thể có ghế nhiều chân hơn nữa, nhưng hiếm. Có các loại ghế "một chân" hay "hai chân" nếu "chân ghế" có hình dạng đủ để tạo thành chân đế bền vững chống đỡ cho cấu trúc không bị đổ.',400000, GETDATE()),
('SP008','GH',N'Ghế cổ điển A',20,'https://cf.shopee.vn/file/d3e13bebaa20adc46dc1c22cd4ec069c',N'Thông thường ghế có bốn chân. Ngoài ra có ghế ba chân và cũng có thể có ghế nhiều chân hơn nữa, nhưng hiếm. Có các loại ghế "một chân" hay "hai chân" nếu "chân ghế" có hình dạng đủ để tạo thành chân đế bền vững chống đỡ cho cấu trúc không bị đổ.',300000, GETDATE()),
('SP009','TU',N'Tủ quần áo A',20,'https://cf.shopee.vn/file/aeb1afae03521ff1135f3982b9cb6b69',N'Tủ là đồ dùng để đựng đồ vật, có hình khối chữ nhật, thường được làm bằng gỗ, hoặc kim loại hay nhựa có cánh cửa và mỗi cánh cửa hay có khóa để giữ an toàn. Có nhiều loại tủ như tủ thờ, tủ quần áo, tủ đựng hàng, tủ đựng tài liệu, tủ đựng đồ dùng, v.v...',250000, GETDATE()),
('SP010','TU',N'Tủ quần áo B',20,'https://cf.shopee.vn/file/aeb1afae03521ff1135f3982b9cb6b69',N'Tủ là đồ dùng để đựng đồ vật, có hình khối chữ nhật, thường được làm bằng gỗ, hoặc kim loại hay nhựa có cánh cửa và mỗi cánh cửa hay có khóa để giữ an toàn. Có nhiều loại tủ như tủ thờ, tủ quần áo, tủ đựng hàng, tủ đựng tài liệu, tủ đựng đồ dùng, v.v...',250000, GETDATE()),
('SP011','TU',N'Tủ bếp C',20,'https://bizweb.dktcdn.net/thumb/large/100/360/933/products/tu-bep-bang-go-tu-nhien-nho-gon-da-nang-tien-loi-ghs-5787-1.jpg?v=1598933882263',N'Tủ là đồ dùng để đựng đồ vật, có hình khối chữ nhật, thường được làm bằng gỗ, hoặc kim loại hay nhựa có cánh cửa và mỗi cánh cửa hay có khóa để giữ an toàn. Có nhiều loại tủ như tủ thờ, tủ quần áo, tủ đựng hàng, tủ đựng tài liệu, tủ đựng đồ dùng, v.v...',250000, GETDATE()),
('SP012','TU',N'Tủ bếp D',20,'https://bizweb.dktcdn.net/thumb/large/100/360/933/products/tu-bep-gia-dinh-go-cong-nghiep-nho-gon-da-nang-ghs-5786-1.jpg?v=1598933931677',N'Tủ là đồ dùng để đựng đồ vật, có hình khối chữ nhật, thường được làm bằng gỗ, hoặc kim loại hay nhựa có cánh cửa và mỗi cánh cửa hay có khóa để giữ an toàn. Có nhiều loại tủ như tủ thờ, tủ quần áo, tủ đựng hàng, tủ đựng tài liệu, tủ đựng đồ dùng, v.v...',250000, GETDATE()),
('SP013','GI',N'Giường ngủ A',20,'https://cf.shopee.vn/file/20b10dabc9c207cd8e8ba1b7fe1f0f8a',N'Giường là một đồ vật hay nơi chốn với cấu tạo chính bằng gỗ hay kim loại, bên trên có trải nệm mút, nệm lò xo hay vạc giường và chiếu. Giường được sử dụng làm nơi ngủ, nằm nghỉ ngơi. Trên giường thường có gối kê, gối ôm, chăn.',600000, GETDATE()),
('SP014','GI',N'Giường ngủ B',20,'https://cf.shopee.vn/file/20b10dabc9c207cd8e8ba1b7fe1f0f8a',N'Giường là một đồ vật hay nơi chốn với cấu tạo chính bằng gỗ hay kim loại, bên trên có trải nệm mút, nệm lò xo hay vạc giường và chiếu. Giường được sử dụng làm nơi ngủ, nằm nghỉ ngơi. Trên giường thường có gối kê, gối ôm, chăn.',600000, GETDATE()),
('SP015','GI',N'Giường ngủ C',20,'https://bizweb.dktcdn.net/thumb/large/100/360/933/products/giuong-ngu-go-chat-luong-cao-cho-gia-dinh-ghs-9071-ava.jpg?v=1565273414883',N'Giường là một đồ vật hay nơi chốn với cấu tạo chính bằng gỗ hay kim loại, bên trên có trải nệm mút, nệm lò xo hay vạc giường và chiếu. Giường được sử dụng làm nơi ngủ, nằm nghỉ ngơi. Trên giường thường có gối kê, gối ôm, chăn.',600000, GETDATE()),
('SP016','GI',N'Giường ngủ D',20,'https://bizweb.dktcdn.net/thumb/large/100/360/933/products/giuong-ngu-go-chat-luong-cao-cho-gia-dinh-ghs-9071-ava.jpg?v=1565273414883',N'Giường là một đồ vật hay nơi chốn với cấu tạo chính bằng gỗ hay kim loại, bên trên có trải nệm mút, nệm lò xo hay vạc giường và chiếu. Giường được sử dụng làm nơi ngủ, nằm nghỉ ngơi. Trên giường thường có gối kê, gối ôm, chăn.',600000, GETDATE()),
('SP017','KE',N'Kệ sách A',20,'https://cf.shopee.vn/file/21e475a8edd1d252c63badce33a2e619',N'Kệ là một mặt phẳng ngang phẳng được sử dụng trong nhà, doanh nghiệp, cửa hàng hoặc nơi khác để giữ các mặt hàng đang được trưng bày, lưu trữ hoặc chào bán. Nó được nâng lên khỏi mặt đất và thường được neo.',200000, GETDATE()),
('SP018','KE',N'Kệ sách B',20,'https://cf.shopee.vn/file/21e475a8edd1d252c63badce33a2e619',N'Kệ là một mặt phẳng ngang phẳng được sử dụng trong nhà, doanh nghiệp, cửa hàng hoặc nơi khác để giữ các mặt hàng đang được trưng bày, lưu trữ hoặc chào bán. Nó được nâng lên khỏi mặt đất và thường được neo.',200000, GETDATE()),
('SP019','KE',N'Kệ sách C',20,'https://cf.shopee.vn/file/21e475a8edd1d252c63badce33a2e619',N'Kệ là một mặt phẳng ngang phẳng được sử dụng trong nhà, doanh nghiệp, cửa hàng hoặc nơi khác để giữ các mặt hàng đang được trưng bày, lưu trữ hoặc chào bán. Nó được nâng lên khỏi mặt đất và thường được neo.',200000, GETDATE()),
('SP020','KE',N'Kệ sách D',20,'https://cf.shopee.vn/file/21e475a8edd1d252c63badce33a2e619',N'Kệ là một mặt phẳng ngang phẳng được sử dụng trong nhà, doanh nghiệp, cửa hàng hoặc nơi khác để giữ các mặt hàng đang được trưng bày, lưu trữ hoặc chào bán. Nó được nâng lên khỏi mặt đất và thường được neo.',200000, GETDATE())


INSERT INTO TINHTRANG(MATINHTRANG,TENTINHTRANG) VALUES
('TT1', N'Giỏ hàng'),
('TT2', N'Đã đặt hàng'),
('TT3', N'Đã duyệt đơn'),
('TT4', N'Đang giao hàng'),
('TT5', N'Đã giao hàng')

INSERT INTO PHUONGTHUCTT(MAPT, TENPT) VALUES
('PT1', N'	Thanh toán khi giao hàng (COD)')


CREATE PROC SP_LAYDANHMUC_SPHAM
	@MALSP VARCHAR(10) = NULL
AS
BEGIN
	IF @MALSP IS NULL
		SELECT * FROM SANPHAM
	ELSE
		SELECT * FROM SANPHAM WHERE MALSP = @MALSP
END

------------------------------------------------------------------------------

SELECT TOP(4) * FROM SANPHAM ORDER BY NGAYTHEM DESC


SELECT * FROM SANPHAM WHERE MALSP = '' 

SELECT TENLSP FROM LOAISP WHERE MALSP = 'BA'

EXEC SP_LAYDANHMUC_SPHAM 'GH'










