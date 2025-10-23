import 'package:flutter/material.dart';
import '../services/user_service.dart';
import '../services/auth_service.dart';
import '../widgets/common_widgets.dart';

class ManageUsersScreen extends StatefulWidget {
  const ManageUsersScreen({super.key});

  @override
  State<ManageUsersScreen> createState() => _ManageUsersScreenState();
}

class _ManageUsersScreenState extends State<ManageUsersScreen> {
  final _svc = UserService();
  final _auth = AuthService();
  late Future<List<Map<String, dynamic>>> _future;
  List<Map<String, dynamic>> _roles = [];

  @override
  void initState() {
    super.initState();
    _future = _svc.list();
    _loadRoles();
  }

  Future<void> _loadRoles() async {
    try {
      _roles = await _auth.roleOptions();
      setState(() {});
    } catch (_) {}
  }

  Future<void> _reload() async {
    setState(() => _future = _svc.list());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Quản lý người dùng')),
      body: RefreshIndicator(
        onRefresh: _reload,
        child: FutureBuilder<List<Map<String, dynamic>>>(
          future: _future,
          builder: (context, snap) {
            if (snap.connectionState == ConnectionState.waiting) {
              return const Center(child: CircularProgressIndicator());
            }
            if (snap.hasError) {
              return Center(child: Text('Lỗi: ${snap.error}'));
            }
            final items = snap.data ?? [];
            if (items.isEmpty) return const Center(child: Text('Chưa có user'));

            return ListView.separated(
              itemCount: items.length,
              separatorBuilder: (_, __) => const Divider(height: 1),
              itemBuilder: (_, i) {
                final u = items[i];
                final id = u['userId'];
                final username = u['username'];
                final roleName = u['roleName'] ?? '';
                final isAdmin = (u['isAdmin'] ?? false) as bool;
                final isActive = (u['isActive'] ?? false) as bool;

                return ListTile(
                  title: Text(username),
                  subtitle: Text('${isAdmin ? "Admin" : "User"} • $roleName'),
                  trailing: Wrap(
                    spacing: 4,
                    children: [
                      IconButton(
                        tooltip: isActive ? 'Khóa' : 'Mở khóa',
                        icon: Icon(
                          isActive ? Icons.lock_open : Icons.lock_outline,
                        ),
                        onPressed: () async {
                          try {
                            await _svc.toggle(id as int);
                            _reload();
                          } catch (e) {
                            // ignore: use_build_context_synchronously
                            showError(context, e);
                          }
                        },
                      ),
                      PopupMenuButton<String>(
                        onSelected: (value) async {
                          try {
                            if (value.startsWith('role:')) {
                              final rid = int.parse(value.split(':')[1]);
                              await _svc.updateRole(id as int, rid);
                              _reload();
                            } else if (value == 'reset') {
                              final ok = await confirm(
                                context,
                                'Đặt lại mật khẩu?',
                                'Sẽ đặt mật khẩu mặc định: Abcd1234@',
                              );
                              if (!ok) return;
                              await _auth.adminResetPassword(
                                id as int,
                                'Abcd1234@',
                              );
                              // ignore: use_build_context_synchronously
                              showOk(context, 'Đã đặt lại mật khẩu');
                            } else if (value == 'delete') {
                              final ok = await confirm(
                                context,
                                'Xoá?',
                                'Xoá user $username ?',
                              );
                              if (!ok) return;
                              await _svc.delete(id as int);
                              _reload();
                            }
                          } catch (e) {
                            // ignore: use_build_context_synchronously
                            showError(context, e);
                          }
                        },
                        itemBuilder: (_) => [
                          PopupMenuItem<String>(
                            value: 'role:-1',
                            enabled: false,
                            child: const Text('Đổi vai trò'),
                          ),
                          ..._roles.map(
                            (r) => PopupMenuItem<String>(
                              value: 'role:${r['roleId']}',
                              child: Text(r['roleName']),
                            ),
                          ),
                          const PopupMenuDivider(),
                          const PopupMenuItem(
                            value: 'reset',
                            child: Text('Đặt lại mật khẩu'),
                          ),
                          const PopupMenuItem(
                            value: 'delete',
                            child: Text('Xoá'),
                          ),
                        ],
                      ),
                    ],
                  ),
                );
              },
            );
          },
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => Navigator.pushNamed(context, '/register'),
        icon: const Icon(Icons.person_add),
        label: const Text('Tạo user'),
      ),
    );
  }
}
