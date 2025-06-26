import {
  ActionState,
  CellType,
  CheckCellApiRequest,
  FieldDto,
} from '../../models/field/fieldMatrix';
import { Field } from '../Field/Field';
import { CheckCell } from '../../endpoints/batleFieldEndpoints';
import { useEffect, useState } from 'react';

type Props = {
  gameField?: FieldDto;
  isReadonly: boolean;
  setIsSucceedCheck?: (isSucceed: ActionState) => void;
};

export function Battlefield(props: Props) {
  const [field, setField] = useState<FieldDto>();

  const checkCell = async (line: number, cell: number) => {
    if (!field) {
      throw new Error('Field is undefined');
    }

    if (field.fieldConfiguration[line][cell] !== CellType.Empty) {
      return;
    }

    const request: CheckCellApiRequest = {
      fieldId: props.gameField?.fieldId as string,
      line,
      cell,
    };

    const result = await CheckCell(request);

    if (result.isSuccessCheck !== ActionState.Success) {
      props.setIsSucceedCheck?.(result.isSuccessCheck);
    }

    setField(result.field);
  };

  useEffect(() => {
    setField(props.gameField);
  }, [props.gameField]);

  return (
    <>
      <Field
        field={field}
        isReadOnly={props.isReadonly}
        checkCell={checkCell}
      />
    </>
  );
}
