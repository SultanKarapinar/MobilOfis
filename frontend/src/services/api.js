import axios from 'axios';
 const api=axios.create({
    baseURL:'http://192.168.2.241:8050',
    timeout:10000,//10sn bekle sonra işlemi durdur
    headers:{'Content-Type':'application/json'}
 });

 export default api;



 