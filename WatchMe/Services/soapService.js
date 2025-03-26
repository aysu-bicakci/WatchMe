const soap = require('soap');
const http = require('http');
const { Pool } = require('pg');
const fs = require('fs');       // Dosya sistemi modülü
const path = require('path');   // Yol modülü

require('dotenv').config(); // dotenv modülünü yükle

// PostgreSQL veritabanı bağlantısı
const pool = new Pool({
  connectionString: process.env.DB_CONNECTION_STRING, // DB bağlantı dizesi .env dosyasından alınır
});

// SOAP servisi tanımı
const service = {
  UserService: {
    UserPort: {
      // Kullanıcı adını almak için metod
      async getUserName({ userId}) {
        try {
          // Veritabanından kullanıcıyı sorgulama
          const result = await pool.query('SELECT name FROM users WHERE id = $1', [1]);
          if (result.rows.length > 0) {
            // Kullanıcı bulunduysa, adını döndür
            return { name: result.rows[0].name };
          } else {
            // Kullanıcı bulunmazsa "Guest" döndür
            return { name: 'Guest' };
          }
        } catch (error) {
          // Veritabanı hatası oluşursa, hata mesajı döndür
          console.error('Database error:', error);
          return { name: 'Error' };
        }
      }
    }
  }
};

// WSDL dosyasını oku
const wsdlFilePath = path.join(__dirname, 'UserService.wsdl');

// WSDL dosyasının mevcut olup olmadığını kontrol et
if (!fs.existsSync(wsdlFilePath)) {
  console.error('WSDL dosyası bulunamadı!');
  process.exit(1); // Eğer WSDL dosyası yoksa, uygulamayı sonlandır
}

const wsdl = fs.readFileSync(wsdlFilePath, 'utf8');

// HTTP sunucusu oluştur
const server = http.createServer((req, res) => {
  res.end('404: Not Found'); // Genel bir 404 yanıtı
});

// SOAP servisini HTTP sunucusuna bağla
soap.listen(server, '/userservice', service, wsdl);

// SOAP servisini 5003 portunda dinlemeye başla
server.listen(5003, () => {
  console.log('SOAP service listening on port 5003');
});