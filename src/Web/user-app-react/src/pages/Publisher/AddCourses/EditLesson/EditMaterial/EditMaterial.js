import classNames from 'classnames/bind';
import { useNavigate, useParams } from 'react-router-dom';

import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';

import styles from './EditMaterial.module.scss';

const cx = classNames.bind(styles);

function EditMaterial({ handleBackStep }) {
    const navigate = useNavigate();
    let params = useParams();

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

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
            <CancelConfirmBtns onCancel={handleCancel} />
        </div>
    );
}

export default EditMaterial;
