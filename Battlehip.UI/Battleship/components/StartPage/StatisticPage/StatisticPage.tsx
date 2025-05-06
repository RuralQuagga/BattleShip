import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../../navigation/MainNavigator';
import { ImageBackground, StyleSheet, View, Text } from 'react-native';
import { Button } from '../../Basic/Button/Button';
import CenteredColumn from '../../Basic/CenteredColumn/CenteredColumn';
import { TableOfContentItem } from '../../Basic/TableOfContentItem/TableOfContentItem';
import { Layout } from '../../Basic/Layout/Layout';
import { globalStyle } from '../../../constants/GlobalStyles';

type Props = NativeStackScreenProps<RootStackParamList, 'StatisticPage'>;

const StatisticPage = ({ navigation }: Props) => {
  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={globalStyle.titleText}>Statistic</Text>
        </View>
        <View style={style.statisticDataContainer}>
          <CenteredColumn style={style.statistic}>
            <TableOfContentItem title='Games' value={14} />
          </CenteredColumn>
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
  titleContainer: {
    flex: 1,
  },  
  statistic: {
    justifyContent: 'flex-start',
  },
  statisticDataContainer: {
    flex: 3,
  },
});

export default StatisticPage;
