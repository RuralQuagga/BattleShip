import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { Layout } from '../Basic/Layout/Layout';
import { View, Text, StyleSheet } from 'react-native';
import { globalStyle } from '../../constants/GlobalStyles';
import { StatisticPanel } from '../StartPage/StatisticPage/StatistiPanel';
import { useEffect, useState } from 'react';
import { ActionState, GameStatistic } from '../../models/field/fieldMatrix';
import { GetSessionStatistic } from '../../endpoints/gameSessionEndpoints';

type Props = NativeStackScreenProps<RootStackParamList, 'GameOverPage'>;

export const GameOverPage = ({ navigation, route }: Props) => {
  const props = route.params;
  const [statistic, setStatistic] = useState<GameStatistic | null>(null);

  const getSessionStatistic = async (sessionId: string) => {
    const response = await GetSessionStatistic(sessionId);
    setStatistic(response);
  };

  const getTitle = (): string => {
    return props.state === ActionState.Win ? 'You Win' : 'You Lose';
  };

  useEffect(() => {
    if (props.sessionId !== null) {
      getSessionStatistic(props.sessionId);
    }
  }, []);

  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>{getTitle()}</Text>
        </View>
        <View style={style.statisticContainer}>
          <View style={style.statisticPanelContainer}>
            {statistic !== null ? (
              <StatisticPanel
                gameTimeMs={statistic.gameTimeMs}
                yourMoves={statistic.yourMoves}
                enemyMoves={statistic.enemyMoves}
                hitPercentage={statistic.hitPercentage}
              />
            ) : (
              <></>
            )}
          </View>
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
  statisticContainer: {
    flex: 1,
  },
  statisticPanelContainer: {
    flex: 3,
  },
});
