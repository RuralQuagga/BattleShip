import axios from 'axios';
import {
  CHANGE_SESSION_STATE_BEGIN,
  CHANGE_SESSION_STATE_END,
  CLEAR_ALL_STATISTIC,
  GET_FULL_STATISTIC,
  GET_SESSION_IN_PROGRESS,
  GET_SESSION_STATISTIC,
  START_GAME_SESSION,
} from '../constants/endpointNames';
import { GameStatistic } from '../models/field/fieldMatrix';

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
  if (sessionId === null) {
    throw new Error(
      `Parameter sessionId of ${SetSessionStatusToInProgress.name} is null`
    );
  }
  const result = await api.post(
    CHANGE_SESSION_STATE_BEGIN + sessionId + CHANGE_SESSION_STATE_END
  );
  if (!result.data) {
    throw new Error(`${SetSessionStatusToInProgress.name} result is undefined`);
  }

  return result;
}

export async function GetSessionInProgress() {
  const result = await api.get(GET_SESSION_IN_PROGRESS);
  if (!result.data) {
    return null;
  }
  return result.data;
}

export async function GetSessionStatistic(
  sessionId: string
): Promise<GameStatistic> {
  if (sessionId === null) {
    throw new Error(
      `Parameter sessionId of ${GetSessionStatistic.name} is null`
    );
  }
  const result = await api.get(
    GET_SESSION_STATISTIC + `?sessionId=${sessionId}`
  );
  if (!result.data) {
    throw new Error(`${GetSessionStatistic.name} result is undefined`);
  }

  return result.data;
}

export async function GetFullStatistic(): Promise<GameStatistic[]> {
  const result = await api.get(GET_FULL_STATISTIC);
  if (!result.data) {
    throw new Error(`${GetFullStatistic.name} result is undefined`);
  }

  return result.data;
}

export async function ClearStatistic() {
  const result = await api.delete(CLEAR_ALL_STATISTIC);
  if (!result.data) {
    throw new Error(`${ClearStatistic.name} result is undefined`);
  }
}
