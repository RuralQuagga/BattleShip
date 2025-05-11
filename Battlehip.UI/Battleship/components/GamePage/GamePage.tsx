import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { Layout } from '../Basic/Layout/Layout';
import { View, Text, StyleSheet, ActivityIndicator } from 'react-native';
import { Battlefield } from '../BattleField/BattleField';
import { globalStyle } from '../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { FieldDto } from '../../models/field/fieldMatrix';
import { GetField } from '../../endpoints/batleFieldEndpoints';
import { SetSessionStatusToInProgress } from '../../endpoints/gameSessionEndpoints';

type Props = NativeStackScreenProps<RootStackParamList, 'GamePage'>;

export const GamePage = ({ navigation, route }: Props) => {
  const { sessionId, field } = route.params;
  const [isSucceed, setIsSucceed] = useState<boolean | null>(null);
  const [fieldState, setFieldState] = useState<FieldDto>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  const setIsSucceedCheck = (isSucceedCheck: boolean) => {
    setIsSucceed(isSucceedCheck);
  };

  const getAndSetComputerField = async () => {
    try {
        if(sessionId === null){
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
    if (isSucceed === false && sessionId !== null) {
      navigation.navigate('UserField', { sessionId: sessionId });
    }
  }, [isSucceed]);

  useEffect(() => {
    SetSessionStatusToInProgress(sessionId);
    if (!field) {
      getAndSetComputerField();
    }else{
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
