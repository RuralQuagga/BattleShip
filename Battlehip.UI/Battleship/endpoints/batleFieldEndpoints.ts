import axios from 'axios';
import { CHECK_CELL, GENERATE_FIELD, REGENERATE_FIELD } from '../constants/endpointNames';
import { CheckCellApiRequest } from '../models/field/fieldMatrix';

const api = axios.create({
    baseURL: 'https://localhost:7166',
  });

export async function GetBattlefield(sessionId: string, isUserField: boolean){
    const result = await api.post(GENERATE_FIELD + `?sessionId=${sessionId}&fieldType=${isUserField ? 1 : 2}`);
    if(!result.data){
      throw new Error(`${GetBattlefield.name} result is undefined`)
    }
    return result;
}

export async function RegenerateBattlefield(fieldId: string) {
  const result = await api.put(REGENERATE_FIELD + `?fieldId=${fieldId}`)
  if(!result.data){
    throw new Error(`${RegenerateBattlefield.name} result is undefined`)
  }
  return result;
}

export async function CheckCell(request:CheckCellApiRequest) {
  const result = await api.post(CHECK_CELL, request);
  if(!result.data){
    throw new Error(`${CheckCell.name} result is undefined`);
  }

  return result;
}