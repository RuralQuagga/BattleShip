import axios from 'axios';
import { START_GAME_SESSION } from '../constants/endpointNames';

const api = axios.create({
    baseURL: 'https://localhost:7166',
  });

  export async function startSession() {
    return api.post(START_GAME_SESSION)
  }