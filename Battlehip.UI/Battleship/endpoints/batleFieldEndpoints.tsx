import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7166',
  });

export async function GetBatlefield(){
    return await api.get('/gameplay/field/generate');
}