import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { useEffect, useState } from 'react';
import {
  ActionState,
  CheckCellApiResponse,
  FieldDto,
} from '../../models/field/fieldMatrix';
import {
  GetField,
  GetFieldAfterComputerMove,
} from '../../endpoints/batleFieldEndpoints';
import { Layout } from '../Basic/Layout/Layout';
import { View, Text, StyleSheet, ActivityIndicator } from 'react-native';
import { Battlefield } from '../BattleField/BattleField';
import { globalStyle } from '../../constants/GlobalStyles';

type Props = NativeStackScreenProps<RootStackParamList, 'UserField'>;

export const UserField = ({ navigation, route }: Props) => {
  const { sessionId } = route.params;
  const [playerField, setPlayerField] = useState<FieldDto>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [polling, setPolling] = useState<boolean>(false);
  const [actionState, setIsSucceed] = useState<ActionState | null>(null);

  const POLLING_INTERVAL = 2000;

  const shouldStopPolling = (response: CheckCellApiResponse): boolean => {
    return (
      response.isSuccessCheck === ActionState.Fail ||
      response.isSuccessCheck === ActionState.Lose ||
      response.isSuccessCheck === ActionState.Win
    );
  };

  const getPlayerFieldOrComputerMove =
    async (): Promise<CheckCellApiResponse> => {
      try {
        if (!playerField) {
          const field = await GetField(sessionId, true);
          console.log('getfield');
          return await GetFieldAfterComputerMove(field.fieldId);
        }

        return await GetFieldAfterComputerMove(playerField.fieldId);
      } catch (err) {
        setError((err as Error).message);
        throw err;
      } finally {
        setLoading(false);
      }
    };

  useEffect(() => {
    let timeoutId: NodeJS.Timeout;
    let isMounted = true;

    const pollData = async () => {
      if (!polling) {
        setPolling(false);
        return;
      }

      try {
        setLoading(true);
        const response = await getPlayerFieldOrComputerMove();

        if (isMounted) {
          setPlayerField(response.field);
          setError('');
          setLoading(false);
          timeoutId = setTimeout(pollData, POLLING_INTERVAL);

          if (shouldStopPolling(response)) {
            setPolling(false);
            setIsSucceedCheck(response.isSuccessCheck);
          }
        } else {
          timeoutId = setTimeout(pollData, POLLING_INTERVAL);
        }
      } catch (err) {
        if (isMounted) {
          setError((err as Error).message);
          setPolling(false);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    if (polling) {
      pollData();
    }

    return () => {
      isMounted = false;
      clearTimeout(timeoutId);
    };
  }, [polling]);

  useEffect(() => {
    setPolling(true);
  }, []);

  const setIsSucceedCheck = (isSucceedCheck: ActionState) => {
    setTimeout(() => setIsSucceed(isSucceedCheck), POLLING_INTERVAL);
  };

  useEffect(() => {
    if (actionState === ActionState.Fail) {
      navigation.navigate('GamePage', { sessionId: sessionId });
    }
    if (actionState === ActionState.Win || actionState === ActionState.Lose) {
      navigation.navigate('GameOverPage', { state: actionState });
    }
  }, [actionState]);

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
          <Text style={[globalStyle.titleText, style.title]}>Your field</Text>
        </View>
        <View style={style.gameFieldContainer}>
          <Battlefield gameField={playerField} isReadonly={true} />
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
