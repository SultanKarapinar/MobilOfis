import axios from 'axios';
import { getToken } from './tokenService'; //getToken fonksiyonu çagrılıyor 

const api = axios.create({
  baseURL: 'http://192.168.2.241:8050',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'//json gonderdım dedım
  }
});
//her ıstek gitmeden once kontrol et 
api.interceptors.request.use(
  async (config) => {

    const token = getToken();

    console.log("GİDEN TOKEN:", token);

    if (token) {
     config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error); //hata olunca axiosa hatayı gerı gonder 
  }
);

export default api;