import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import {
  ActivityIndicator,
  ImageBackground,
  StyleSheet,
  Text,
  View,
} from 'react-native';
import CenteredColumn from '../Basic/CenteredColumn/CenteredColumn';
import { Button } from '../Basic/Button/Button';
import { globalStyle } from '../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { GetSessionInProgress } from '../../endpoints/gameSessionEndpoints';

type Props = NativeStackScreenProps<RootStackParamList, 'StartPage'>;

const StartPage = ({ navigation }: Props) => {
  const [lastSessionId, setLastSessionId] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');

  const getLastSessionId = async () => {
    try {
      const result = await GetSessionInProgress();
      setLastSessionId(result as string);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    getLastSessionId();
  }, []);

  if (loading) {
    return <ActivityIndicator size='large' />;
  }

  if (error !== '') {
    return <Text>Ошибка: {error}</Text>;
  }

  return (
    <>
      <ImageBackground
        source={require('../../assets/startPageBackground.svg')}
        style={style.imageBackground}
        resizeMode='cover'
      >
        <View style={style.container}>
          <View style={style.titleContainer}>
            <Text style={globalStyle.titleText}>BattleShip</Text>
          </View>
          <View style={style.menuContainer}>
            <CenteredColumn style={style.menu}>
              <Button
                title='Play'
                btnStyle={style.btn}
                textStyle={style.btnName}
                onPress={() => navigation.navigate('PlayPage')}
              />
              <Button
                disabled={lastSessionId === null}
                title='Continue'
                btnStyle={style.btn}
                textStyle={style.btnName}
                onPress={() =>
                  navigation.navigate('GamePage', { sessionId: lastSessionId })
                }
              />
              <Button
                title='Statistic'
                btnStyle={style.btn}
                textStyle={style.btnName}
                onPress={() => navigation.navigate('StatisticPage')}
              />
              <Button
                title='Info'
                btnStyle={style.btn}
                textStyle={style.btnName}
                onPress={() => {}}
              />
            </CenteredColumn>
          </View>
        </View>
      </ImageBackground>
    </>
  );
};

const style = StyleSheet.create({
  container: {
    flex: 1,
  },
  imageBackground: {
    flex: 1,
    width: '100%',
    height: '100%',
  },
  titleContainer: {
    flex: 1,
  },
  menuContainer: {
    flex: 3,
  },
  menu: {
    justifyContent: 'flex-start',
    marginTop: '25%',
  },
  btn: {
    width: '65%',
    backgroundColor: 'rgba(138,213,238,0.9)',
    borderBlockColor: '#002C4C',
    borderWidth: 1,
  },
  btnName: {
    fontSize: 20,
    fontWeight: 600,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',
  },
});

export default StartPage;
