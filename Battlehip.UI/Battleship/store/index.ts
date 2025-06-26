import { configureStore } from '@reduxjs/toolkit';
import SessionReducer from './reducers/gameSessionReducer';

export const store = configureStore({
  reducer: {
    session: SessionReducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware(),
  devTools: process.env.NODE_ENV !== 'production',
});
