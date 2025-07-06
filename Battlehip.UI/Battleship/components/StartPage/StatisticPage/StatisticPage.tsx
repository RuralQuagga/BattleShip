import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../../navigation/MainNavigator';
import { StyleSheet, View, Text } from 'react-native';
import { Layout } from '../../Basic/Layout/Layout';
import { globalStyle } from '../../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { GameStatistic } from '../../../models/field/fieldMatrix';
import {
  ClearStatistic,
  GetFullStatistic,
} from '../../../endpoints/gameSessionEndpoints';
import { StatisticPanel } from '../StatisticPage/StatistiPanel';
import { Button } from '../../Basic/Button/Button';
import { CustomScrollView } from '../../Basic/ScrollView/CustomScrollView';

type Props = NativeStackScreenProps<RootStackParamList, 'StatisticPage'>;

const StatisticPage = ({ navigation }: Props) => {
  const [fullStatistic, setFullStatistic] = useState<GameStatistic[] | null>(
    null
  );

  const getFullStatistic = async () => {
    const response = await GetFullStatistic();
    setFullStatistic(response);
  };

  const clearStatistic = async () => {
    await ClearStatistic();
    setFullStatistic(null);
  };

  useEffect(() => {
    getFullStatistic();
  }, []);

  useEffect(() => {}, [fullStatistic]);

  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>Statistic</Text>
        </View>
        <View style={style.countContainer}>
          <Text style={style.text}>Total Games: {fullStatistic?.length}</Text>
        </View>
        {fullStatistic === null || fullStatistic.length === 0 ? (
          <View style={{ flex: 6 }} />
        ) : (
          <View style={style.statisticDataContainer}>
            <CustomScrollView contentContainerStyle={style.statistic}>
              {fullStatistic !== null ? (
                fullStatistic.map((value, index) => (
                  <View style={style.container} key={index}>
                    <StatisticPanel
                      gameTimeMs={value.gameTimeMs}
                      yourMoves={value.yourMoves}
                      enemyMoves={value.enemyMoves}
                      hitPercentage={value.hitPercentage}
                    />
                  </View>
                ))
              ) : (
                <></>
              )}
            </CustomScrollView>
          </View>
        )}
        <View style={style.clearBtn}>
          <Button
            title='Clear'
            imgSource={require('../../../assets/trash-alt-svgrepo-com.svg')}
            btnStyle={style.btnStyle}
            onPress={() => clearStatistic()}
          />
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
  container: {
    width: '90%',
    margin: 3,
  },
  text: {
    flex: 1,
    margin: 20,
    fontSize: 20,
    fontWeight: 200,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',
  },
  statistic: {
    alignItems: 'center',
    flex: 1,
    padding: 16,
  },
  titleContainer: {
    flex: 1,
  },
  countContainer: {
    flex: 1,
    justifyContent: 'center',
    alignContent: 'space-between',
  },
  title: {
    marginTop: '10%',
  },
  statisticDataContainer: {
    flex: 6,
    backgroundColor: 'rgba(101, 153, 170, 0.69)',
    margin: 3,
    borderRadius: 10,
    paddingBottom: 16,
  },
  clearBtn: {
    flex: 1,
  },
  btnStyle: {
    width: '20%',
    backgroundColor: 'rgba(138,213,238,0.9)',
    borderBlockColor: '#002C4C',
    padding: 10,
  },
});

export default StatisticPage;
