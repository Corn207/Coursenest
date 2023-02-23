import classNames from 'classnames/bind';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';

import styles from './EditMaterial.module.scss';

const cx = classNames.bind(styles);

function EditMaterial() {
    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Title</p>
            <p className={cx('description')}>Description</p>
            <div className={cx('contentContainer')}>
                <p className={cx('content')}>Content</p>
                <textarea
                    className={cx('contentText')}
                    placeholder="“Give any additional context on what happened.”"
                ></textarea>
            </div>
            <CancelConfirmBtns />
        </div>
    );
}

export default EditMaterial;
