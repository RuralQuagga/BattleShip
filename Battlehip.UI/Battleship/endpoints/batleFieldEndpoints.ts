import axios from 'axios';
import { GENERATE_FIELD } from '../constants/endpointNames';

const api = axios.create({
    baseURL: 'https://localhost:7166',
  });

export async function GetBatlefield(sessionId: string, isUserField: boolean){
    return await api.post(GENERATE_FIELD+`?sessionId=${sessionId}&fieldType=${isUserField ? 1 : 2}`);
}