const express = require('express');
const axios = require('axios');
const cors = require('cors');

const app = express();
const PORT = 5002;

// CORS ayarı (frontend'in API'ye erişmesine izin verir)
app.use(cors());

// TMDb API anahtarını burada tanımla
const TMDB_API_KEY = '06aa759d714112a56cfa3b9c5fc7ab3a';

// Keşfet rotası: TMDb'den popüler filmleri getirir
app.get('/discover', async (req, res) => {
    try {
        const response = await axios.get(`https://api.themoviedb.org/3/movie/popular`, {
            params: {
                api_key: TMDB_API_KEY,
                language: 'tr-TR',
                page: 1
            }
        });

        res.json(response.data);
    } catch (error) {
        console.error('API isteğinde hata:', error);
        res.status(500).json({ message: 'Sunucu hatası. Lütfen daha sonra tekrar deneyin.' });
    }
});

// Sunucuyu başlat
app.listen(PORT, () => {
    console.log(`Node.js API çalışıyor: http://localhost:${PORT}`);
});