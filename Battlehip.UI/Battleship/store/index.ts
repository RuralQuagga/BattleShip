import { configureStore, createSlice } from '@reduxjs/toolkit';

interface DefauilReducerState {
    value: number;
  }

const initialState: DefauilReducerState = { value: 0 }
const defaultReducer = createSlice({
    name: 'defaultReducer',
    initialState,
    reducers: {
        add: (state) => {
            state.value += 1;
        },
    },
})

export const store = configureStore({
  reducer: {
    default: defaultReducer.reducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware(),
  devTools: process.env.NODE_ENV !== 'production',
});
