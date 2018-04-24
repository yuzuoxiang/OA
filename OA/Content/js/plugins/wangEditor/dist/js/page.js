(function () {

    // ��ȡ wangEditor ���캯���� jquery
    var E = window.wangEditor;
    var $ = window.jQuery;

    // �� createMenu ���������˵�
    E.createMenu(function (check) {

        // ����˵�id����Ҫ�������˵�id�ظ����༭���Դ������в˵�id����ͨ������������-�Զ���˵���һ�ڲ鿴
        var menuId = 'page';

        // check�����˵����ã�����������-�Զ���˵���һ�����������Ƿ�ò˵�id�����û�У����������Ĵ��롣
        if (!check(menuId)) {
            return;
        }

        // this ָ�� editor ��������
        var editor = this;

        // ���� menu ����
        var menu = new E.Menu({
            editor: editor,  // �༭������
            id: menuId,  // �˵�id
            title: '��ҳ��', // �˵�����

            // ����״̬��ѡ��״̬�µ�dom������ʽ��Ҫ�Զ���
            $domNormal: $('<a href="#" tabindex="-1"><i class="wangeditor-menu-img-indent-left"></i></a>'),
            $domSelected: $('<a href="#" tabindex="-1" class="selected"><i class="wangeditor-menu-img-indent-left"></i></a>')
        });

        // �˵�����״̬�£�������������¼�
        menu.clickEvent = function (e) {
            // �ҵ���ǰѡ�����ڵ� p Ԫ��
            var elem = editor.getRangeElem();
            var p = editor.getSelfOrParentByName(elem, 'p');
            var $p;

            if (!p) {
                // δ�ҵ� p Ԫ�أ������
                return e.preventDefault();
            }
            $p = $(p);

            // ʹ���Զ�������
            function commandFn() {
                $p.append('<hr>');
            }
            editor.customCommand(e, commandFn);
        };

        // �˵�ѡ��״̬�£�������������¼�
        menu.clickEventSelected = function (e) {
            // �ҵ���ǰѡ�����ڵ� p Ԫ��
            var elem = editor.getRangeElem();
            var p = editor.getSelfOrParentByName(elem, 'p');
            var $p;

            if (!p) {
                // δ�ҵ� p Ԫ�أ������
                return e.preventDefault();
            }
            $p = $(p);

            // ʹ���Զ�������
            function commandFn() {
                 $p.remove('<hr>');
            }
            editor.customCommand(e, commandFn);
        };

        // ���ݵ�ǰѡ�����Զ�����²˵���ѡ��״̬��������״̬
        menu.updateSelectedEvent = function () {
            // ��ȡ��ǰѡ�����ڵĸ�Ԫ��
            var elem = editor.getRangeElem();
            var p = editor.getSelfOrParentByName(elem, 'p');
            var $p;
            var indent;

            if (!p) {
                // δ�ҵ� p Ԫ�أ�����Ϊδ����ѡ��״̬
                return false;
            }
            $p = $(p);
            indent = $p.css('text-indent');

            if (!indent || indent === '0px') {
                // �õ���p��text-indent ������ 0������Ϊδ����ѡ��״̬
                return false;
            }

            // �ҵ� p Ԫ�أ����� text-indent ���� 0������Ϊѡ��״̬
            return true;
        };

        // ���ӵ�editor������
        editor.menus[menuId] = menu;
    });

})();