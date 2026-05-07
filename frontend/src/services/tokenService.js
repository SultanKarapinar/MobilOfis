let authToken = null;
//sadece ram de calısır 

export const setToken = (token) => {
  console.log("TOKEN KAYDEDİLDİ:", token);
  authToken = token;
};

export const getToken = () => {
  return authToken;
  //hafızadakı token ı gerı donduruyor 
};

export const clearToken = () => {
  authToken = null; 
  //tokenı sılıyor 
};