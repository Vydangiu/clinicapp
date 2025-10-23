// import 'package:flutter/material.dart';
// import '../services/patient_service.dart';
// import '../widgets/common_widgets.dart';
// import 'patient_edit_screen.dart';

// class PatientsScreen extends StatefulWidget {
//   const PatientsScreen({super.key});

//   @override
//   State<PatientsScreen> createState() => _PatientsScreenState();
// }

// class _PatientsScreenState extends State<PatientsScreen> {
//   final _svc = PatientService();
//   late Future<List<Map<String, dynamic>>> _future;

//   @override
//   void initState() {
//     super.initState();
//     _future = _svc.getAll();
//   }

//   Future<void> _reload() async {
//     setState(() => _future = _svc.getAll());
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(title: const Text('Bệnh nhân')),
//       body: RefreshIndicator(
//         onRefresh: _reload,
//         child: FutureBuilder<List<Map<String, dynamic>>>(
//           future: _future,
//           builder: (context, snap) {
//             if (snap.connectionState == ConnectionState.waiting) {
//               return const Center(child: CircularProgressIndicator());
//             }
//             if (snap.hasError) {
//               return Center(child: Text('Lỗi: ${snap.error}'));
//             }
//             final items = snap.data ?? [];
//             if (items.isEmpty) {
//               return const Center(child: Text('Chưa có dữ liệu'));
//             }

//             return ListView.separated(
//               itemCount: items.length,
//               separatorBuilder: (_, __) => const Divider(height: 1),
//               itemBuilder: (_, i) {
//                 final p = items[i];
//                 final id = p['patientId'] ?? p['PatientID'] ?? p['id'];
//                 final name = (p['fullName'] ?? p['FullName'] ?? 'N/A')
//                     .toString();
//                 final phone = (p['phone'] ?? '').toString();

//                 return ListTile(
//                   title: Text(name),
//                   subtitle: Text(phone),
//                   onTap: () async {
//                     final updated = await Navigator.push<Map<String, dynamic>>(
//                       context,
//                       MaterialPageRoute(
//                         builder: (_) => PatientEditScreen(data: p),
//                       ),
//                     );
//                     if (updated != null) _reload();
//                   },
//                   trailing: IconButton(
//                     icon: const Icon(Icons.delete),
//                     onPressed: () async {
//                       final ok = await confirm(
//                         context,
//                         'Xoá?',
//                         'Xoá bệnh nhân $name ?',
//                       );
//                       if (!ok) return;
//                       try {
//                         await _svc.delete(id as int);
//                         // ignore: use_build_context_synchronously
//                         showOk(context, 'Đã xoá');
//                         _reload();
//                       } catch (e) {
//                         // ignore: use_build_context_synchronously
//                         showError(context, e);
//                       }
//                     },
//                   ),
//                 );
//               },
//             );
//           },
//         ),
//       ),
//       floatingActionButton: FloatingActionButton.extended(
//         onPressed: () async {
//           final created = await Navigator.push<Map<String, dynamic>>(
//             context,
//             MaterialPageRoute(
//               builder: (_) => const PatientEditScreen(data: null),
//             ),
//           );
//           if (created != null) _reload();
//         },
//         label: const Text('Thêm'),
//         icon: const Icon(Icons.add),
//       ),
//     );
//   }
// }

import 'package:flutter/material.dart';
import '../services/patient_service.dart';
import '../widgets/common_widgets.dart';
import 'patient_edit_screen.dart';

class PatientsScreen extends StatefulWidget {
  const PatientsScreen({super.key});

  @override
  State<PatientsScreen> createState() => _PatientsScreenState();
}

class _PatientsScreenState extends State<PatientsScreen> {
  final _svc = PatientService();
  late Future<List<Map<String, dynamic>>> _future;

  @override
  void initState() {
    super.initState();
    _future = _svc.getAll();
  }

  Future<void> _reload() async {
    setState(() => _future = _svc.getAll());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Bệnh nhân'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () async {
              await _svc.logout();
              if (!mounted) return;
              // ignore: use_build_context_synchronously
              Navigator.pushNamedAndRemoveUntil(context, '/', (_) => false);
            },
          ),
        ],
      ),
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
            if (items.isEmpty) {
              return const Center(child: Text('Chưa có dữ liệu'));
            }

            return ListView.separated(
              itemCount: items.length,
              separatorBuilder: (_, __) => const Divider(height: 1),
              itemBuilder: (_, i) {
                final p = items[i];
                final id = p['patientId'] ?? p['PatientID'] ?? p['id'];
                final name = (p['fullName'] ?? p['FullName'] ?? 'N/A')
                    .toString();
                final phone = (p['phone'] ?? '').toString();

                return ListTile(
                  title: Text(name),
                  subtitle: Text(phone.isEmpty ? '—' : phone),
                  onTap: () async {
                    final updated = await Navigator.push<Map<String, dynamic>>(
                      context,
                      MaterialPageRoute(
                        builder: (_) => PatientEditScreen(data: p),
                      ),
                    );
                    if (updated != null) _reload();
                  },
                  trailing: IconButton(
                    icon: const Icon(Icons.delete),
                    onPressed: () async {
                      final ok = await confirm(
                        context,
                        'Xoá?',
                        'Xoá bệnh nhân $name ?',
                      );
                      if (!ok) return;
                      try {
                        await _svc.delete(id as int);
                        if (context.mounted) showOk(context, 'Đã xoá');
                        _reload();
                      } catch (e) {
                        if (context.mounted) showError(context, e);
                      }
                    },
                  ),
                );
              },
            );
          },
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () async {
          final created = await Navigator.push<Map<String, dynamic>>(
            context,
            MaterialPageRoute(
              builder: (_) => const PatientEditScreen(data: null),
            ),
          );
          if (created != null) _reload();
        },
        label: const Text('Thêm'),
        icon: const Icon(Icons.add),
      ),
    );
  }
}
