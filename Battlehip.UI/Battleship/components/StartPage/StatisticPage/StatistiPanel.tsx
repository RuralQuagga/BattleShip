import { StyleSheet, View } from 'react-native';
import { GameStatistic } from '../../../models/field/fieldMatrix';
import { formatDuration } from '../../../hooks/timeHooks';
import { TableOfContentItem } from '../../Basic/TableOfContentItem/TableOfContentItem';
import CenteredColumn from '../../Basic/CenteredColumn/CenteredColumn';

type Props = GameStatistic;

export const StatisticPanel = (props: Props) => {
  return (
    <>
      <CenteredColumn style={style.statistic}>
        <TableOfContentItem
          title='Game Time'
          value={formatDuration(props.gameTimeMs)}
        />
        <TableOfContentItem title='Your Moves' value={props.yourMoves} />
        <TableOfContentItem title='Enemy Moves' value={props.enemyMoves} />
        <TableOfContentItem
          title='Hit Percentage'
          value={props.hitPercentage}
        />
      </CenteredColumn>
    </>
  );
};

const style = StyleSheet.create({
  statistic: {
    flex: 1,
    height: 100,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 10,
    backgroundColor: 'rgba(138,213,238,0.8)',
  },
});
