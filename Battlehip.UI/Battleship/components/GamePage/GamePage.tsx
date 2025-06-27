import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { Layout } from '../Basic/Layout/Layout';
import { View, Text, StyleSheet, ActivityIndicator } from 'react-native';
import { Battlefield } from '../BattleField/BattleField';
import { globalStyle } from '../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { ActionState, FieldDto } from '../../models/field/fieldMatrix';
import { GetField } from '../../endpoints/batleFieldEndpoints';
import { SetSessionStatusToInProgress } from '../../endpoints/gameSessionEndpoints';
import { useAppDispatch, useAppSelector } from '../../store/type';
import { updateSession } from '../../store/reducers/gameSessionReducer';
import {
  Session,
  SessionState,
} from '../../models/gameSession/gameSessionTypes';

type Props = NativeStackScreenProps<RootStackParamList, 'GamePage'>;

export const GamePage = ({ navigation, route }: Props) => {
  const { sessionId, field } = route.params;
  const session = useAppSelector((state) => state.session);
  const dispatch = useAppDispatch();
  const [actionState, setActionState] = useState<ActionState | null>(null);
  const [fieldState, setFieldState] = useState<FieldDto>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  const setIsSucceedCheck = (isSucceedCheck: ActionState) => {
    setActionState(isSucceedCheck);
  };

  const changeSessionState = async (sessionId: string | null) => {
    if (session.state === SessionState.InProgress || sessionId === null) return;
    const updatedSession = await SetSessionStatusToInProgress(sessionId);
    dispatch(updateSession(updatedSession.data as Session));
  };

  const getAndSetComputerField = async () => {
    try {
      if (sessionId === null) {
        return;
      }
      const field = await GetField(sessionId, false);
      setFieldState(field);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (actionState === ActionState.Fail && sessionId !== null) {
      navigation.navigate('UserField', { sessionId: sessionId });
    }
    if (
      sessionId !== null &&
      (actionState === ActionState.Lose || actionState === ActionState.Win)
    ) {
      navigation.navigate('GameOverPage', {
        state: actionState,
        sessionId: sessionId,
      });
    }
  }, [actionState]);

  useEffect(() => {
    changeSessionState(sessionId);
    if (!field) {
      getAndSetComputerField();
    } else {
      setFieldState(field);
      setLoading(false);
    }
  }, []);

  if (loading) {
    return <ActivityIndicator size='large' />;
  }

  if (error !== '') {
    return <Text>Ошибка: {error}</Text>;
  }

  return (
    <>
      <Layout>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>Game</Text>
        </View>
        <View style={style.gameFieldContainer}>
          <Battlefield
            gameField={fieldState}
            isReadonly={false}
            setIsSucceedCheck={setIsSucceedCheck}
          />
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
  titleContainer: {
    flex: 1,
  },
  title: {
    marginTop: '10%',
  },
  gameFieldContainer: {
    flex: 5,
  },
});
