import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../../navigation/MainNavigator';
import { Layout } from '../../Basic/Layout/Layout';
import { Battlefield } from '../../BattleField/BattleField';
import { StyleSheet, View, Text, ActivityIndicator } from 'react-native';
import { globalStyle } from '../../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { startSession } from '../../../endpoints/gameSessionEndpoints';
import { addNew } from '../../../store/reducers/gameSessionReducer';
import { Button } from '../../Basic/Button/Button';
import { FieldDto } from '../../../models/field/fieldMatrix';
import {
  GetBattlefield,
  RegenerateBattlefield,
} from '../../../endpoints/batleFieldEndpoints';
import { Session } from '../../../models/gameSession/gameSessionTypes';
import { useAppDispatch } from '../../../store/type';

type Props = NativeStackScreenProps<RootStackParamList, 'PlayPage'>;

export const PlayPage = ({ navigation }: Props) => {
  const dispatch = useAppDispatch();
  const [sessionId, setSessionId] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [myField, setMyField] = useState<FieldDto>();

  const fetchDataField = async () => {
    try {
      const response = await GetBattlefield(sessionId, true);
      setMyField(response.data as FieldDto);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  const fetchData = async () => {
    try {
      const response = await startSession();
      setSessionId((response.data as Session).id);
      dispatch(addNew(response.data as Session));
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  const regenerateDataField = async (fieldId: string | undefined) => {
    try {
      if (!fieldId) {
        throw new Error('Field Id is undefined');
      }
      const response = await RegenerateBattlefield(fieldId);
      setMyField(response.data as FieldDto);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };
  const startGame = async (fieldId: string | undefined) => {
    try {
      if (!fieldId) {
        throw new Error('Field Id is undefined');
      }
      const response = await GetBattlefield(sessionId, false);
      navigation.navigate('GamePage', {
        field: response.data as FieldDto,
        sessionId: sessionId,
      });
      setMyField(response.data as FieldDto);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (sessionId) {
      fetchDataField();
    } else {
      fetchData();
    }
  }, [sessionId]);

  if (loading) {
    return <ActivityIndicator size='large' />;
  }

  if (error !== '') {
    return <Text>Ошибка: {error}</Text>;
  }
  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>
            Prepare to game
          </Text>
        </View>
        <View style={style.playContainer}>
          <Battlefield gameField={myField} isReadonly={true} />
          <View style={style.btnContainer}>
            <Button
              title='Regenerate'
              onPress={() => regenerateDataField(myField?.fieldId)}
              imgSource={require('../../../assets/recycling-svgrepo-com.svg')}
              btnStyle={style.btnStyle}
            />
            <Button
              title='start'
              onPress={() => startGame(myField?.fieldId)}
              imgSource={require('../../../assets/right-chevron-svgrepo-com.svg')}
              btnStyle={style.btnStyle}
            />
          </View>
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
  playContainer: {
    flex: 5,
  },
  titleContainer: {
    flex: 1,
  },
  title: {
    marginTop: '10%',
  },
  btnContainer: {
    flexDirection: 'column',
    alignContent: 'flex-end',
  },
  btnStyle: {
    width: '20%',
    backgroundColor: 'rgba(138,213,238,0.9)',
    borderBlockColor: '#002C4C',
    padding: 10,
  },
  btnName: {
    fontSize: 30,
    fontWeight: 600,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',
  },
});
