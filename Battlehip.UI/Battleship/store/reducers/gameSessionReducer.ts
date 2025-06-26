import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {
  Session,
  SessionState,
} from '../../models/gameSession/gameSessionTypes';

const initialState: Session = {
  id: '',
  sessionEnd: null,
  sessionStart: null,
  state: null,
};
const SessionReducer = createSlice({
  name: 'SessionId',
  initialState,
  reducers: {
    addNew: (state, action: PayloadAction<Session>) => {
      return { ...state, ...action.payload };
    },
    updateSession: (state, action: PayloadAction<Session>) => {
      return { ...state, ...action.payload };
    },
  },
});

export const { addNew, updateSession } = SessionReducer.actions;
export default SessionReducer.reducer;
