import axios from 'axios';
import { CHANGE_SESSION_STATE_BEGIN, CHANGE_SESSION_STATE_END, GET_SESSION_IN_PROGRESS, START_GAME_SESSION } from '../constants/endpointNames';

const api = axios.create({
  baseURL: 'https://localhost:7166',
});

export async function startSession() {
  const result = await api.post(START_GAME_SESSION);
  if (!result.data) {
    throw new Error(`${startSession.name} result is undefined`);
  }
  return result;
}

export async function SetSessionStatusToInProgress(sessionId: string | null) {
  if(sessionId === null){
    throw new Error(`Parameter sessionId of ${SetSessionStatusToInProgress.name} is null`)
  }
  const result = await api.post(CHANGE_SESSION_STATE_BEGIN + sessionId + CHANGE_SESSION_STATE_END);
  if(!result.data){
    throw new Error(`${SetSessionStatusToInProgress.name} result is undefined`);
  }

  return result;
}

export async function GetSessionInProgress() {
  const result = await api.get(GET_SESSION_IN_PROGRESS);
  if(!result.data){
    return null;
  }
  return result.data;
}
